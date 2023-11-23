using OVB.Demos.Eschody.Domain.StudentContext;

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
        var idStudent = Guid.NewGuid();
        var correlationId = Guid.NewGuid();
        var executionUser = "UserTest";
        var sourcePlatform = ".NET/UnitTests.cs";
        var createdAt = DateTime.UtcNow;

        // Act
        var student = new Student(
            id: idStudent,
            correlationId: correlationId,
            sourcePlatform: sourcePlatform,
            executionUser: executionUser,
            createdAt: createdAt,
            firstName: firstName,
            lastName: lastName,
            email: email, 
            phone: phone,
            password: password,
            lastCorrelationId: correlationId,
            lastSourcePlatform: sourcePlatform,
            lastExecutionUser: executionUser,
            lastModifiedAt: createdAt);

        // Assert
        Assert.Equal(
           expected: idStudent,
           actual: student.Id);
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
           expected: createdAt,
           actual: student.CreatedAt);
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
           expected: createdAt,
           actual: student.LastModifiedAt);
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
