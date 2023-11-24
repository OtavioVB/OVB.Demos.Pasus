using OVB.Demos.Eschody.Domain.ValueObjects;

namespace OVB.Demos.Eschody.UnitTests.Domain.ValueObjects;

public sealed class EmailValueObjectTests
{
    [Theory]
    [InlineData("otaviovb.developer@gmail.com")]
    [InlineData("otavio-villas@gmail.com")]
    [InlineData("otavio-.villas@gmail.com")]
    [InlineData("o@m.c")]
    public void Test_EmailShouldBeValid(string email)
    {
        // Arrange
        var emailValueObject = EmailValueObject.Build(email);

        // Act

        // Assert
        Assert.True(emailValueObject.IsValid);
        Assert.True(emailValueObject.GetProcessResult().IsSuccess);
        Assert.False(emailValueObject.GetProcessResult().IsPartial);
        Assert.False(emailValueObject.GetProcessResult().IsError);
        Assert.Empty(emailValueObject.GetProcessResult().Notifications ?? []);
    }

    [Theory]
    [InlineData("k@m")]
    [InlineData("otavio eschody")]
    [InlineData("otavio@ com")]
    public void Test_EmailShouldBeNotValid(string email)
    {
        // Arrange
        var emailValueObject = EmailValueObject.Build(email);

        // Act

        // Assert
        Assert.False(emailValueObject.IsValid);
        Assert.False(emailValueObject.GetProcessResult().IsSuccess);
        Assert.False(emailValueObject.GetProcessResult().IsPartial);
        Assert.True(emailValueObject.GetProcessResult().IsError);
    }

    [Theory]
    [InlineData("k@c")]
    [InlineData("k@cd")]
    [InlineData("k")]
    [InlineData("")]
    public void Test_EmailReturnLessThanLengthMessage(string email)
    {
        // Arrange
        var emailValueObject = EmailValueObject.Build(email);

        // Act

        // Assert
        Assert.Contains(emailValueObject.GetProcessResult().Notifications!, p => p.Message == "O email precisa conter pelo menos 5 caracteres.");
        Assert.Contains(emailValueObject.GetProcessResult().Notifications!, p => p.Code == "ESC06");
    }

    [Theory]
    [InlineData("IDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDSIDSAJIDYUDSAUHDS")]
    public void Test_EmailReturnGreaterThanLengthMessage(string email)
    {
        // Arrange
        var emailValueObject = EmailValueObject.Build(email);

        // Act

        // Assert
        Assert.Contains(emailValueObject.GetProcessResult().Notifications!, p => p.Message == "O email precisa conter até 255 caracteres.");
        Assert.Contains(emailValueObject.GetProcessResult().Notifications!, p => p.Code == "ESC05");
    }

    [Theory]
    [InlineData("otaviocarmanini")]
    [InlineData("otaviocarmanini.com")]
    public void Test_EmailReturnIsNotValidMessage(string email)
    {
        // Arrange
        var emailValueObject = EmailValueObject.Build(email);

        // Act

        // Assert
        Assert.Contains(emailValueObject.GetProcessResult().Notifications!, p => p.Message == "O email inserido precisa ser válido.");
        Assert.Contains(emailValueObject.GetProcessResult().Notifications!, p => p.Code == "ESC07");
    }
}
