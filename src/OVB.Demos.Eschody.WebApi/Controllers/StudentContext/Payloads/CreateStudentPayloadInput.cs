namespace OVB.Demos.Eschody.WebApi.Controllers.StudentContext.Payloads;

public readonly struct CreateStudentPayloadInput
{
    public CreateStudentPayloadInput(string firstName, string lastName, string email, string password, string phone, int typeStudent)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Phone = phone;
        TypeStudent = typeStudent;
    }

    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string Phone { get; init; }
    public int TypeStudent { get; init; }
}
