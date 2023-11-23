using Microsoft.AspNetCore.Mvc;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using OVB.Demos.Eschody.WebApi.Controllers.Base.Models;

namespace OVB.Demos.Eschody.WebApi.Controllers.Base;

public abstract class CustomControllerBase : ControllerBase
{
    public CustomUnprocessableEntityResult GetUnprocessableEntityForInvalidAuditable()
        => CustomUnprocessableEntityResult.Build(
            propertyName: nameof(AuditableInfoValueObject),
            propertyDescription: "The auditable info data is invalid.");
}
