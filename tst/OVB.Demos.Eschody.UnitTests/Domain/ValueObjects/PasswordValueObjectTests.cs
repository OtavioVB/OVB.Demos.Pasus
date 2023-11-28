using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Security.Cryptography;

namespace OVB.Demos.Eschody.UnitTests.Domain.ValueObjects;

public sealed class PasswordValueObjectTests
{
    [Theory]
    [InlineData("45134456")]
    public void Password_Should_Be_Valid(string password)
    {
        // Arrange
        var passwordValueObject = PasswordValueObject.Build(password, false);

        var encodedValue = Encoding.UTF8.GetBytes(password);
        var encryptedPassword = SHA256.HashData(encodedValue);
        var sb = new StringBuilder();
        foreach (var caracter in encryptedPassword) sb.Append(caracter.ToString("X2"));

        // Assert
        Assert.Equal(sb.ToString(), passwordValueObject.GetPassword());
        Assert.True(passwordValueObject.IsValid);
        Assert.True(passwordValueObject.GetProcessResult().IsSuccess);
        Assert.False(passwordValueObject.GetProcessResult().IsPartial);
        Assert.False(passwordValueObject.GetProcessResult().IsError);
    }

    [Theory]
    [InlineData("45454")]
    [InlineData("teste")]
    public void Phone_Should_Be_Not_Valid(string password)
    {
        // Arrange
        var passwordValueObject = PasswordValueObject.Build(password, false);

        // Assert
        Assert.Throws<EschodyValueObjectException>(passwordValueObject.GetPassword);
        Assert.False(passwordValueObject.IsValid);
        Assert.False(passwordValueObject.GetProcessResult().IsSuccess);
        Assert.False(passwordValueObject.GetProcessResult().IsPartial);
        Assert.True(passwordValueObject.GetProcessResult().IsError);
    }
}
