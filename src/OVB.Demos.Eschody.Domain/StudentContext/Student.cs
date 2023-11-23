namespace OVB.Demos.Eschody.Domain.StudentContext;

public sealed record Student
{
    public Student(
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

    public Guid Id { get; set; }

    public Guid CorrelationId { get; set; }
    public string SourcePlatform { get; set; }
    public string ExecutionUser { get; set; }
    public DateTime CreatedAt { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }

    public Guid LastCorrelationId { get; set; }
    public string LastSourcePlatform { get; set; }
    public string LastExecutionUser { get; set; }
    public DateTime LastModifiedAt { get; set; }
}
