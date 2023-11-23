using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace OVB.Demos.Eschody.WebApi.Controllers.StudentContext;

[Route("api/v1/backoffice/students")]
[ApiController]
public sealed class StudentController : ControllerBase
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
        [FromHeader(Name = "X-Idempotency-Key")] string idempotencyKey,
        [FromHeader(Name = "X-Correlation-Id")] Guid correlationId,
        [FromHeader(Name = "X-Source-Platform")] string sourcePlatform,
        [FromHeader(Name = "X-Execution-User")] string executionUser,
        CancellationToken cancellationToken)
    {
        return StatusCode(StatusCodes.Status503ServiceUnavailable, null);
    }
}
