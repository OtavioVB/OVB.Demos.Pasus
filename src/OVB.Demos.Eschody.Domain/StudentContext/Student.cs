using OVB.Demos.Eschody.Libraries.ValueObjects;
using System.Data;

namespace OVB.Demos.Eschody.Domain.StudentContext;

public sealed record Student
{
    private Student(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }

    public Guid? CorrelationId { get; private set; }
    public string? SourcePlatform { get; private set; }
    public string? ExecutionUser { get; private set; }
    public DateTime? CreatedAt { get; private set; }

    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Password { get; private set; }

    public Guid? LastCorrelationId { get; private set; }
    public string? LastSourcePlatform { get; private set; }
    public string? LastExecutionUser { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }

    private void SetAuditableInfo(AuditableInfoValueObject auditableInfo)
    {
        CorrelationId = auditableInfo.CorrelationId;
        ExecutionUser = auditableInfo.ExecutionUser;
        SourcePlatform = auditableInfo.SourcePlatform;
        CreatedAt = DateTime.UtcNow;
        LastModifiedAt = DateTime.UtcNow;
        LastCorrelationId = auditableInfo.CorrelationId;
        LastExecutionUser = auditableInfo.ExecutionUser;
        LastSourcePlatform = auditableInfo.SourcePlatform;
    }

    private void SetContent(string firstName, string lastName, string email, string phone, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Password = password;
    }

    public static Student BuildStudent(
        AuditableInfoValueObject auditableInfo,
        string firstName,
        string lastName,
        string email,
        string phone,
        string password)
    {
       var student = new Student(
            id: Guid.NewGuid());
        student.SetAuditableInfo(auditableInfo);
        student.SetContent(firstName, lastName, email, phone, password);
        return student;
    }
}
