using Olizia.Demos.Resale.ValueObjects.Exceptions;
using OVB.Demos.Eschody.Domain.ValueObjects;
using System.Globalization;

namespace OVB.Demos.Eschody.UnitTests.Domain.ValueObjects;

public sealed class FirstNameValueObjectTests
{

    [Theory]
    [InlineData("otavio")]
    [InlineData("Otavio")]
    [InlineData("Otávio")]
    [InlineData("Otávio Augusto")]
    public void FirstName_Should_Be_Valid(string firstName)
    {
        // Arrange
        var firstNameValueObject = FirstNameValueObject.Build(firstName);
        var cultureInfo = CultureInfo.GetCultureInfo("pt-BR");

        // Act
        var firstNameTitleCase = cultureInfo.TextInfo.ToTitleCase(firstName);

        // Assert
        Assert.Equal(firstNameTitleCase, firstNameValueObject.GetFirstName());
        Assert.True(firstNameValueObject.IsValid);
        Assert.True(firstNameValueObject.GetProcessResult().IsSuccess);
        Assert.False(firstNameValueObject.GetProcessResult().IsPartial);
        Assert.False(firstNameValueObject.GetProcessResult().IsError);
    }

    [Theory]
    [InlineData("Otavio Augusto Carmanini Silva da Costa")]
    [InlineData("O")]
    public void FirstName_Should_Be_Not_Valid(string firstName)
    {
        // Arrange
        var firstNameValueObject = FirstNameValueObject.Build(firstName);
        var cultureInfo = CultureInfo.GetCultureInfo("pt-BR");

        // Act
        var firstNameTitleCase = cultureInfo.TextInfo.ToTitleCase(firstName);

        // Assert
        Assert.Throws<EschodyValueObjectException>(() => firstNameValueObject.GetFirstName());
        Assert.False(firstNameValueObject.IsValid);
        Assert.False(firstNameValueObject.GetProcessResult().IsSuccess);
        Assert.False(firstNameValueObject.GetProcessResult().IsPartial);
        Assert.True(firstNameValueObject.GetProcessResult().IsError);
    }
}
