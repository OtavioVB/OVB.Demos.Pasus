using OVB.Demos.Eschody.Domain.StudentContext.DataTransferObject;
using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Domain.StudentContext.Functions.CreateStudent.Outputs;

public readonly struct CreateStudentDomainFunctionResult
{
    private CreateStudentDomainFunctionResult(
        Guid studentId, FirstNameValueObject firstName, LastNameValueObject lastName, EmailValueObject email, PhoneValueObject phone, PasswordValueObject password, AuditableInfoValueObject auditableInfo)
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

    public static CreateStudentDomainFunctionResult Build(Guid studentId, FirstNameValueObject firstName, LastNameValueObject lastName, EmailValueObject email, PhoneValueObject phone, PasswordValueObject password, AuditableInfoValueObject auditableInfo)
        => new CreateStudentDomainFunctionResult(studentId, firstName, lastName, email, phone, password, auditableInfo);

    public Student Adapt()
        => Student.BuildStudent(
            auditableInfo: AuditableInfo,
            firstName: FirstName.GetFirstName(),
            lastName: LastName.GetLastName(),
            email: Email.GetEmail(),
            phone: Phone.GetPhone(),
            password: Password.GetPassword());
}
