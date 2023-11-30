using OVB.Demos.Eschody.Domain.StudentContext.ENUMs;
using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Application.Services.Internal.StudentContext.Inputs;

public readonly struct CreateStudentServiceInput
{
    public CreateStudentServiceInput(AuditableInfoValueObject auditableInfo, FirstNameValueObject firstName, LastNameValueObject lastName, EmailValueObject email, PhoneValueObject phone, PasswordValueObject password, TypeStudent typeStudent)
    {
        AuditableInfo = auditableInfo;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Password = password;
        TypeStudent = typeStudent;
    }

    public AuditableInfoValueObject AuditableInfo { get; }
    public FirstNameValueObject FirstName { get; }
    public LastNameValueObject LastName { get; }
    public EmailValueObject Email { get; }
    public PhoneValueObject Phone { get; }
    public PasswordValueObject Password { get; }
    public TypeStudent TypeStudent { get; }

    public static CreateStudentServiceInput Build(AuditableInfoValueObject auditableInfo, FirstNameValueObject firstName, LastNameValueObject lastName, EmailValueObject email, PhoneValueObject phone, PasswordValueObject password, TypeStudent typeStudent)
        => new CreateStudentServiceInput(auditableInfo, firstName, lastName, email, phone, password, typeStudent);
}
