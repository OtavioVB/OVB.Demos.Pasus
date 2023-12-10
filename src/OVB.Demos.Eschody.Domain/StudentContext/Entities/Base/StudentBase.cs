using OVB.Demos.Eschody.Domain.Notifications;
using OVB.Demos.Eschody.Domain.StudentContext.ENUMs;
using OVB.Demos.Eschody.Domain.StudentContext.Functions.CreateStudent.Inputs;
using OVB.Demos.Eschody.Domain.StudentContext.Functions.CreateStudent.Interfaces;
using OVB.Demos.Eschody.Domain.StudentContext.Functions.CreateStudent.Outputs;
using OVB.Demos.Eschody.Libraries.NotificationContext;
using OVB.Demos.Eschody.Libraries.ProcessResultContext;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Domain.StudentContext.Entities.Base;

public abstract class StudentBase : ICreateStudentDomainFunction
{
    private readonly TypeStudent _type;

    protected StudentBase(TypeStudent type)
    {
        _type = type;
    }

    public async Task<ProcessResult<Notification, CreateStudentDomainFunctionResult>> CreateStudentDomainFunctionAsync(
        CreateStudentDomainFunctionInput input,
        Func<string, AuditableInfoValueObject, CancellationToken, Task<bool>> verifyStudentExistsByEmail,
        CancellationToken cancellationToken)
    {
        var initialValidation = ProcessResult<Notification, CreateStudentDomainFunctionResult>.BuildFromAnotherProcessResult(
            output: default,
            input.AuditableInfo.GetProcessResult(),
            input.FirstName.GetProcessResult(),
            input.LastName.GetProcessResult(),
            input.Email.GetProcessResult(),
            input.Phone.GetProcessResult(),
            input.Password.GetProcessResult());

        if (initialValidation.IsError)
            return initialValidation;

        if (initialValidation.IsPartial)
            throw new NotImplementedException();

        var studentExistsByEmail = await verifyStudentExistsByEmail(
            arg1: input.Email.GetEmail(),
            arg2: input.AuditableInfo,
            arg3: cancellationToken);

        var businessValidations = ProcessResult<Notification, CreateStudentDomainFunctionResult>.BuildFromAnotherProcessResult(
            output: default,
            processResults: ValidateThatStudentDoesNotExistsYet(
                studentExists: studentExistsByEmail));

        if (businessValidations.IsError)
            return businessValidations;

        if (businessValidations.IsPartial)
            throw new NotImplementedException();

        return ProcessResult<Notification, CreateStudentDomainFunctionResult>.BuildSuccessfullProcessResult(
            output: CreateStudentDomainFunctionResult.Build(
                studentId: GenerateStudentId(),
                firstName: input.FirstName,
                lastName: input.LastName,
                email: input.Email,
                phone: input.Phone,
                password: input.Password,
                auditableInfo: input.AuditableInfo));
    }

    protected static Guid GenerateStudentId()
        => Guid.NewGuid();

    protected virtual ProcessResult<Notification> ValidateThatStudentDoesNotExistsYet(bool studentExists, int? index = null)
    {
        if (studentExists)
            return ProcessResult<Notification>.BuildErrorfullProcessResult(
                notifications: [
                    NotificationFacilitator.StudentExistDomain(index)
                ],
                exceptions: null);
        else
            return ProcessResult<Notification>.BuildSuccessfullProcessResult(
                notifications: null,
                exceptions: null);
    }

    public static StudentBase Build(TypeStudent typeStudent)
    {
        switch (typeStudent)
        {
            case TypeStudent.Standard:
                return new StudentStandard();
            default:
                throw new NotImplementedException();
        }
    }
}
