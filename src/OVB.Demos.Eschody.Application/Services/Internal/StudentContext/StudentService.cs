using OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Inputs;
using OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Interfaces;
using OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Outputs;
using OVB.Demos.Eschody.Domain.StudentContext.DataTransferObject;
using OVB.Demos.Eschody.Domain.StudentContext.Entities.Base;
using OVB.Demos.Eschody.Domain.StudentContext.Functions.CreateStudent.Inputs;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Base.Interfaces;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using System.Diagnostics;

namespace OVB.Demos.Eschody.Application.Services.Internal.StudentContext;

public sealed class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExtensionStudentRepository _extensionStudentRepository;
    private readonly IBaseRepository<Student> _baseStudentRepository;
    private readonly ITraceManager _traceManager;

    public StudentService(
        IUnitOfWork unitOfWork, 
        IExtensionStudentRepository extensionStudentRepository, 
        IBaseRepository<Student> baseStudentRepository, 
        ITraceManager traceManager)
    {
        _unitOfWork = unitOfWork;
        _extensionStudentRepository = extensionStudentRepository;
        _baseStudentRepository = baseStudentRepository;
        _traceManager = traceManager;
    }

    public Task<ProcessResult<Notification, CreateStudentServiceResult>> CreateStudentServiceAsync(
        CreateStudentServiceInput input, CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: $"{nameof(StudentService)}.{nameof(CreateStudentServiceAsync)}",
            activityKind: ActivityKind.Internal,
            input: input,
            handler: async (input, auditableInfo, activity, cancellationToken) =>
            {
                var studentDomain = StudentBase.Build(
                    typeStudent: input.TypeStudent);

                var createStudentDomainResult = await studentDomain.CreateStudentDomainFunctionAsync(
                    input: CreateStudentDomainFunctionInput.Build(
                        auditableInfo: input.AuditableInfo,
                        firstName: input.FirstName,
                        lastName: input.LastName,
                        email: input.Email,
                        phone: input.Phone,
                        password: input.Password),
                    verifyStudentExistsByEmail: _extensionStudentRepository.VerifyStudentExistsByEmailAsync,
                    cancellationToken: cancellationToken);

                if (createStudentDomainResult.IsError)
                    return ProcessResult<Notification, CreateStudentServiceResult>.BuildErrorfullProcessResult(
                        output: default,
                        notifications: createStudentDomainResult.Notifications,
                        exceptions: createStudentDomainResult.Exceptions);

                if (createStudentDomainResult.IsPartial)
                    throw new NotImplementedException();

                await _baseStudentRepository.AddAsync(
                    entity: createStudentDomainResult.Output.Adapt(),
                    auditableInfo: input.AuditableInfo,
                    cancellationToken: cancellationToken);
                await _unitOfWork.ApplyDatabaseTransactionAsync(cancellationToken);

                return ProcessResult<Notification, CreateStudentServiceResult>.BuildSuccessfullProcessResult(
                    output: CreateStudentServiceResult.BuildFromDomainResult(createStudentDomainResult.Output),
                    notifications: createStudentDomainResult.Notifications,
                    exceptions: createStudentDomainResult.Exceptions);
            },
            auditableInfo: input.AuditableInfo,
            cancellationToken: cancellationToken);
}
