using OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Inputs;
using OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Interfaces;
using OVB.Demos.Eschody.Application.UseCases.CreateStudent.Inputs;
using OVB.Demos.Eschody.Application.UseCases.CreateStudent.Outputs;
using OVB.Demos.Eschody.Application.UseCases.Interfaces;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using System.Diagnostics;

namespace OVB.Demos.Eschody.Application.UseCases.CreateStudent;

public sealed class CreateStudentUseCase : IUseCase<CreateStudentUseCaseInput, CreateStudentUseCaseResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStudentService _studentService;
    private readonly ITraceManager _traceManager;

    public CreateStudentUseCase(
        IUnitOfWork unitOfWork,
        IStudentService studentService,
        ITraceManager traceManager)
    {
        _unitOfWork = unitOfWork;
        _studentService = studentService;
        _traceManager = traceManager;
    }

    public Task<ProcessResult<Notification, CreateStudentUseCaseResult>> ExecuteUseCaseAsync(
        CreateStudentUseCaseInput input, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken)
        => _traceManager.ExecuteTraceAsync(
            traceName: $"{nameof(CreateStudentUseCase)}.{nameof(ExecuteUseCaseAsync)}",
            activityKind: ActivityKind.Internal,
            input: input,
            handler: (inputBody, inputAuditableInfo, activity, inputCancellationToken) 
                => _unitOfWork.ExecuteUnitOfWorkAsync(
                    handler: async (cancellationToken) =>
                    {
                        var createStudentServiceResult = await _studentService.CreateStudentServiceAsync(
                            input: CreateStudentServiceInput.Build(
                                auditableInfo: inputAuditableInfo,
                                firstName: inputBody.FirstName,
                                lastName: inputBody.LastName,
                                email: inputBody.Email,
                                phone: inputBody.Phone,
                                password: inputBody.Password,
                                typeStudent: inputBody.TypeStudent),
                            cancellationToken: cancellationToken);

                        if (createStudentServiceResult.IsError)
                            return (false, ProcessResult<Notification, CreateStudentUseCaseResult>.BuildErrorfullProcessResult(
                                output: default,
                                notifications: createStudentServiceResult.Notifications,
                                exceptions: createStudentServiceResult.Exceptions));

                        if (createStudentServiceResult.IsPartial)
                            throw new NotImplementedException();

                        return (true, ProcessResult<Notification, CreateStudentUseCaseResult>.BuildSuccessfullProcessResult(
                            output: CreateStudentUseCaseResult.Build(createStudentServiceResult.Output.StudentId),
                            notifications: createStudentServiceResult.Notifications,
                            exceptions: createStudentServiceResult.Exceptions));
                    }, inputCancellationToken),
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);
}
