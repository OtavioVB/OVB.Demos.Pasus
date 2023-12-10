using OVB.Demos.Eschody.Domain.StudentContext.ENUMs;
using OVB.Demos.Eschody.Domain.ValueObjects;

namespace OVB.Demos.Eschody.Application.UseCases.StudentContext.CreateStudent.Inputs;

public readonly struct CreateStudentUseCaseInput
{
    private CreateStudentUseCaseInput(
        FirstNameValueObject firstName,
        LastNameValueObject lastName,
        EmailValueObject email,
        PhoneValueObject phone,
        PasswordValueObject password,
        TypeStudent typeStudent)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Password = password;
        TypeStudent = typeStudent;
    }

    public FirstNameValueObject FirstName { get; }
    public LastNameValueObject LastName { get; }
    public EmailValueObject Email { get; }
    public PhoneValueObject Phone { get; }
    public PasswordValueObject Password { get; }
    public TypeStudent TypeStudent { get; }

    public static CreateStudentUseCaseInput Build(FirstNameValueObject firstName, LastNameValueObject lastName, EmailValueObject email,
        PhoneValueObject phone, PasswordValueObject password, TypeStudent typeStudent)
        => new CreateStudentUseCaseInput(firstName, lastName, email, phone, password, typeStudent);
}
