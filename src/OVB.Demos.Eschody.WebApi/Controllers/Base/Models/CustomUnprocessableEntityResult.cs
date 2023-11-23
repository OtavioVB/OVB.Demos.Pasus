namespace OVB.Demos.Eschody.WebApi.Controllers.Base.Models;

public readonly struct CustomUnprocessableEntityResult
{
    private CustomUnprocessableEntityResult(string propertyName, string propertyDescription)
    {
        PropertyName = propertyName;
        PropertyDescription = propertyDescription;
    }

    public string PropertyName { get; }
    public string PropertyDescription { get; }

    public static CustomUnprocessableEntityResult Build(string propertyName, string propertyDescription)
        => new CustomUnprocessableEntityResult(propertyName, propertyDescription);
}
