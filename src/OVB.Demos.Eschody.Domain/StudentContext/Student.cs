using System.Data;

namespace OVB.Demos.Eschody.Domain.StudentContext;

public sealed record Student
{
    private Student(
        Guid id, 
        
        Guid correlationId, string sourcePlatform, string executionUser, DateTime createdAt, 
        
        string firstName, string lastName, string email, string phone, string password,
        
        Guid lastCorrelationId, string lastSourcePlatform, string lastExecutionUser, DateTime lastModifiedAt)
    {
        Id = id;
        CorrelationId = correlationId;
        SourcePlatform = sourcePlatform;
        ExecutionUser = executionUser;
        CreatedAt = createdAt;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Password = password;
        LastCorrelationId = lastCorrelationId;
        LastSourcePlatform = lastSourcePlatform;
        LastExecutionUser = lastExecutionUser;
        LastModifiedAt = lastModifiedAt;
    }

    public Guid Id { get; }

    public Guid CorrelationId { get; }
    public string SourcePlatform { get; }
    public string ExecutionUser { get; }
    public DateTime CreatedAt { get; }

    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Phone { get; }
    public string Password { get; }

    public Guid LastCorrelationId { get; }
    public string LastSourcePlatform { get; }
    public string LastExecutionUser { get; }
    public DateTime LastModifiedAt { get; }

    public static Student BuildStudent(
        Guid correlationId,
        string sourcePlatform,
        string executionUser,
        string firstName,
        string lastName,
        string email,
        string phone,
        string password) => new Student(
            id: Guid.NewGuid(),
            correlationId: correlationId,
            sourcePlatform: sourcePlatform,
            executionUser: executionUser,
            createdAt: DateTime.UtcNow,
            firstName: firstName,
            lastName: lastName, 
            email: email,
            phone: phone,
            password: password,
            lastCorrelationId: correlationId,
            lastSourcePlatform: sourcePlatform,
            lastExecutionUser: executionUser,
            lastModifiedAt: DateTime.UtcNow);
}
