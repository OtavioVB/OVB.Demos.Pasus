using OVB.Demos.Eschody.Domain.StudentContext.Functions.CreateStudent.Outputs;
using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Outputs;

public readonly struct CreateStudentServiceResult
{
    private CreateStudentServiceResult(Guid studentId, FirstNameValueObject firstName, LastNameValueObject lastName, EmailValueObject email, PhoneValueObject phone, PasswordValueObject password, AuditableInfoValueObject auditableInfo)
    {
        StudentId = studentId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Password = password;
        AuditableInfo = auditableInfo;
    }

    public Guid StudentId { get; }
    public FirstNameValueObject FirstName { get; }
    public LastNameValueObject LastName { get; }
    public EmailValueObject Email { get; }
    public PhoneValueObject Phone { get; }
    public PasswordValueObject Password { get; }
    public AuditableInfoValueObject AuditableInfo { get; }

    public static CreateStudentServiceResult Build(Guid studentId, FirstNameValueObject firstName, LastNameValueObject lastName, EmailValueObject email, PhoneValueObject phone, PasswordValueObject password, AuditableInfoValueObject auditableInfo)
        => new CreateStudentServiceResult(studentId, firstName, lastName, email, phone, password, auditableInfo);

    public static CreateStudentServiceResult BuildFromDomainResult(CreateStudentDomainFunctionResult result)
        => new CreateStudentServiceResult(
            studentId: result.StudentId,
            firstName: result.FirstName,
            lastName: result.LastName,
            email: result.Email,
            phone: result.Phone,
            password: result.Password,
            auditableInfo: result.AuditableInfo);
}
