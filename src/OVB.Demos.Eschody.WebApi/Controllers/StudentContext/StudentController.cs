using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using OVB.Demos.Eschody.WebApi.Controllers.Base;
using System.Net.Mime;

namespace OVB.Demos.Eschody.WebApi.Controllers.StudentContext;

[Route("api/v1/backoffice/students")]
[ApiController]
public sealed class StudentController : CustomControllerBase
{
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [AllowAnonymous]
    public async Task<IActionResult> HttpPostCreateStudentServiceAsync(
        [FromHeader(Name = HttpIdempotencyKeyTitle)] string idempotencyKey,
        [FromHeader(Name = HttpCorrelationIdTitle)] Guid correlationId,
        [FromHeader(Name = HttpSourcePlatformTitle)] string sourcePlatform,
        [FromHeader(Name = HttpExecutionUserTitle)] string executionUser,
        CancellationToken cancellationToken)
    {
        var auditableInfo = AuditableInfoValueObject.Build(
            correlationId: correlationId,
            sourcePlatform: sourcePlatform,
            executionUser: executionUser,
            requestedAt: DateTime.UtcNow);
        if (!auditableInfo.IsValid)
            return StatusCode(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                value: GetUnprocessableEntityForInvalidAuditable());

        AddAuditableInfoAtHeadersResponse(HttpContext.Response, auditableInfo, idempotencyKey);


        return StatusCode(StatusCodes.Status503ServiceUnavailable, null);
    }
}
