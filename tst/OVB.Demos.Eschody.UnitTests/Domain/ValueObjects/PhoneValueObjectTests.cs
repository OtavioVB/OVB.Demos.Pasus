using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;
using System.Globalization;

namespace OVB.Demos.Eschody.UnitTests.Domain.ValueObjects;

public sealed class PhoneValueObjectTests
{
    [Theory]
    [InlineData("11999999999")]
    public void Phone_Should_Be_Valid(string phone)
    {
        // Arrange
        var phoneValueObject = PhoneValueObject.Build(phone);

        // Assert
        Assert.Equal(phone, phoneValueObject.GetPhone());
        Assert.True(phoneValueObject.IsValid);
        Assert.True(phoneValueObject.GetProcessResult().IsSuccess);
        Assert.False(phoneValueObject.GetProcessResult().IsPartial);
        Assert.False(phoneValueObject.GetProcessResult().IsError);
    }

    [Theory]
    [InlineData("119999999a9")]
    [InlineData("119999999449")]
    [InlineData("999999949")]
    public void Phone_Should_Be_Not_Valid(string phone)
    {
        // Arrange
        var phoneValueObject = PhoneValueObject.Build(phone);

        // Assert
        Assert.Throws<PasusValueObjectException>(phoneValueObject.GetPhone);
        Assert.False(phoneValueObject.IsValid);
        Assert.False(phoneValueObject.GetProcessResult().IsSuccess);
        Assert.False(phoneValueObject.GetProcessResult().IsPartial);
        Assert.True(phoneValueObject.GetProcessResult().IsError);
    }
}
