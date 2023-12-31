﻿using OVB.Demos.Eschody.Domain.StudentContext.DataTransferObject;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.UnitTests.Domain.StudentContext.DataTransferObject;

public sealed class StudentDataTransferObjectTests
{
    [Theory]
    [InlineData("Otavio", "Carmanini", "otavio@gmail.com", "11999999999", "123456789")]
    [InlineData("", "", "", "", "")]
    [InlineData("dfdas45543543", "dsa4354353**&", "¨$%¨$%¨$DS", "4345uthfdh", "<script>console.log(\"etc\");</script>")]
    public void Test_RegularWorkflowAtDataTransferObjectCreation(string firstName, string lastName, string email, string phone, string password)
    {
        // Arrange
        var correlationId = Guid.NewGuid();
        var executionUser = "UserTest";
        var sourcePlatform = ".NET/UnitTests.cs";
        var auditableInfo = AuditableInfoValueObject.Build(correlationId, sourcePlatform, executionUser, DateTime.UtcNow, "1233154356");

        // Act
        var student = Student.BuildStudent(
            auditableInfo: auditableInfo,
            firstName: firstName,
            lastName: lastName,
            email: email, 
            phone: phone,
            password: password);

        // Assert
        Assert.NotEqual(
            expected: Guid.Empty,
            actual: student.Id);
        Assert.Equal(
            expected: auditableInfo.GetRequestedAt().ToString("dd/MM/yyyy HH:mm"),
            actual: student.CreatedAt!.Value.ToString("dd/MM/yyyy HH:mm"));
        Assert.Equal(
            expected: auditableInfo.GetRequestedAt().ToString("dd/MM/yyyy HH:mm"),
            actual: student.LastModifiedAt!.Value.ToString("dd/MM/yyyy HH:mm"));
        Assert.Equal(
           expected: correlationId,
           actual: student.CorrelationId);
        Assert.Equal(
           expected: executionUser,
           actual: student.ExecutionUser);
        Assert.Equal(
           expected: sourcePlatform,
           actual: student.SourcePlatform);
        Assert.Equal(
           expected: correlationId,
           actual: student.LastCorrelationId);
        Assert.Equal(
           expected: executionUser,
           actual: student.LastExecutionUser);
        Assert.Equal(
           expected: sourcePlatform,
           actual: student.LastSourcePlatform);
        Assert.Equal(
            expected: firstName, 
            actual: student.FirstName);
        Assert.Equal(
           expected: lastName,
           actual: student.LastName);
        Assert.Equal(
           expected: email,
           actual: student.Email);
        Assert.Equal(
           expected: password,
           actual: student.Password);
        Assert.Equal(
           expected: phone,
           actual: student.Phone);
    }
}
