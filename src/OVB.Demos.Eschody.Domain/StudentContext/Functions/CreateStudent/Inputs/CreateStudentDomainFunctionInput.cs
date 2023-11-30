using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Domain.StudentContext.Functions.CreateStudent.Inputs;

public readonly struct CreateStudentDomainFunctionInput
{
    private CreateStudentDomainFunctionInput(
        AuditableInfoValueObject auditableInfo, 
        FirstNameValueObject firstName, 
        LastNameValueObject lastName, 
        EmailValueObject email, 
        PhoneValueObject phone, 
        PasswordValueObject password)
    {
        AuditableInfo = auditableInfo;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Password = password;
    }

    public AuditableInfoValueObject AuditableInfo { get; }
    public FirstNameValueObject FirstName { get; }
    public LastNameValueObject LastName { get; }
    public EmailValueObject Email { get; }
    public PhoneValueObject Phone { get; }
    public PasswordValueObject Password { get; }

    public static CreateStudentDomainFunctionInput Build(AuditableInfoValueObject auditableInfo, FirstNameValueObject firstName,
        LastNameValueObject lastName, EmailValueObject email, PhoneValueObject phone, PasswordValueObject password)
        => new CreateStudentDomainFunctionInput(auditableInfo, firstName, lastName, email, phone, password);
}
