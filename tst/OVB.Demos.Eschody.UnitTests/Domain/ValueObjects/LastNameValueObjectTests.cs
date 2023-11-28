using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;
using System.Globalization;

namespace OVB.Demos.Eschody.UnitTests.Domain.ValueObjects;

public sealed class LastNameValueObjectTests
{
    [Theory]
    [InlineData("carmanini")]
    [InlineData("Al")]
    [InlineData("silva")]
    public void LastName_Should_Be_Valid(string lastName)
    {
        // Arrange
        var lastNameValueObject = LastNameValueObject.Build(lastName);
        var cultureInfo = CultureInfo.GetCultureInfo("pt-BR");

        // Act
        var lastNameTitleCase = cultureInfo.TextInfo.ToTitleCase(lastName);

        // Assert
        Assert.Equal(lastNameTitleCase, lastNameValueObject.GetLastName());
        Assert.True(lastNameValueObject.IsValid);
        Assert.True(lastNameValueObject.GetProcessResult().IsSuccess);
        Assert.False(lastNameValueObject.GetProcessResult().IsPartial);
        Assert.False(lastNameValueObject.GetProcessResult().IsError);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("")]
    [InlineData("0123456789012345678901234567890123456789")]
    public void LastName_Should_Be_Not_Valid(string lastName)
    {
        // Arrange
        var lastNameValueObject = LastNameValueObject.Build(lastName);
        var cultureInfo = CultureInfo.GetCultureInfo("pt-BR");

        // Act
        var lastNameTitleCase = cultureInfo.TextInfo.ToTitleCase(lastName);

        // Assert
        Assert.Throws<EschodyValueObjectException>(lastNameValueObject.GetLastName);
        Assert.False(lastNameValueObject.IsValid);
        Assert.False(lastNameValueObject.GetProcessResult().IsSuccess);
        Assert.False(lastNameValueObject.GetProcessResult().IsPartial);
        Assert.True(lastNameValueObject.GetProcessResult().IsError);
    }
}
