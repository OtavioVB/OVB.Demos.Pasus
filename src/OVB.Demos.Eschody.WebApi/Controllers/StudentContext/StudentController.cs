using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OVB.Demos.Eschody.Application.UseCases.CreateStudent.Inputs;
using OVB.Demos.Eschody.Application.UseCases.CreateStudent.Outputs;
using OVB.Demos.Eschody.Application.UseCases.Interfaces;
using OVB.Demos.Eschody.Domain.StudentContext.ENUMs;
using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using OVB.Demos.Eschody.WebApi.Controllers.Base;
using OVB.Demos.Eschody.WebApi.Controllers.StudentContext.Payloads;
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
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [AllowAnonymous]
    public async Task<IActionResult> HttpPostCreateStudentServiceAsync(
        [FromServices] IUseCase<CreateStudentUseCaseInput, CreateStudentUseCaseResult> useCase,
        [FromHeader(Name = AuditableInfoValueObject.IdempotencyKey)] string idempotencyKey,
        [FromHeader(Name = AuditableInfoValueObject.CorrelationIdKey)] Guid correlationId,
        [FromHeader(Name = AuditableInfoValueObject.SourcePlatformKey)] string sourcePlatform,
        [FromHeader(Name = AuditableInfoValueObject.ExecutionUserKey)] string executionUser,
        [FromHeader(Name = AuditableInfoValueObject.RequestedAtKey)] DateTime requestedAt,
        [FromBody] CreateStudentPayloadInput input,
        CancellationToken cancellationToken)
    {
        var auditableInfo = AuditableInfoValueObject.Build(
            correlationId: correlationId,
            sourcePlatform: sourcePlatform,
            executionUser: executionUser,
            requestedAt: requestedAt);
        if (!auditableInfo.IsValid)
            return StatusCode(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                value: GetUnprocessableEntityForInvalidAuditable());

        AddAuditableInfoAtHeadersResponse(HttpContext.Response, auditableInfo, idempotencyKey);

        var useCaseResult = await useCase.ExecuteUseCaseAsync(
            input: CreateStudentUseCaseInput.Build(
                firstName: FirstNameValueObject.Build(input.FirstName),
                lastName: LastNameValueObject.Build(input.LastName),
                email: EmailValueObject.Build(input.Email),
                phone: PhoneValueObject.Build(input.Phone),
                password: PasswordValueObject.Build(input.Password, false),
                typeStudent: (TypeStudent)input.TypeStudent),
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);

        if (useCaseResult.IsSuccess == true)
            return StatusCode(
                statusCode: StatusCodes.Status201Created,
                value: useCaseResult.Output);

        if (useCaseResult.IsPartial)
            return StatusCode(StatusCodes.Status503ServiceUnavailable, null);

        return StatusCode(
            statusCode: StatusCodes.Status400BadRequest, 
            value: useCaseResult.Notifications);
    }
}
