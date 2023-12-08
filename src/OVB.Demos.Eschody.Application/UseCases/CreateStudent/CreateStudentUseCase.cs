using OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Inputs;
using OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Interfaces;
using OVB.Demos.Eschody.Application.UseCases.CreateStudent.Inputs;
using OVB.Demos.Eschody.Application.UseCases.CreateStudent.Outputs;
using OVB.Demos.Eschody.Application.UseCases.Interfaces;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Application.UseCases.CreateStudent;

public sealed class CreateStudentUseCase : IUseCase<CreateStudentUseCaseInput, CreateStudentUseCaseResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStudentService _studentService;

    public CreateStudentUseCase(
        IUnitOfWork unitOfWork,
        IStudentService studentService)
    {
        _unitOfWork = unitOfWork;
        _studentService = studentService;
    }

    public Task<ProcessResult<Notification, CreateStudentUseCaseResult>> ExecuteUseCaseAsync(
        CreateStudentUseCaseInput input, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken)
        => _unitOfWork.ExecuteUnitOfWorkAsync(
            handler: async (cancellationToken) =>
            {
                var createStudentServiceResult = await _studentService.CreateStudentServiceAsync(
                    input: CreateStudentServiceInput.Build(
                        auditableInfo: auditableInfo,
                        firstName: input.FirstName,
                        lastName: input.LastName,
                        email: input.Email,
                        phone: input.Phone,
                        password: input.Password,
                        typeStudent: input.TypeStudent),
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
            }, cancellationToken);
}
