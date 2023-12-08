using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OVB.Demos.Eschody.Application.UseCases.CreateStudent.Inputs;
using OVB.Demos.Eschody.Application.UseCases.CreateStudent.Outputs;
using OVB.Demos.Eschody.Application.UseCases.Interfaces;
using OVB.Demos.Eschody.Domain.StudentContext.ENUMs;
using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Interfaces;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Models;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using OVB.Demos.Eschody.WebApi.Controllers.Base;
using OVB.Demos.Eschody.WebApi.Controllers.StudentContext.Payloads;
using System.Net.Mime;
using System.Text.Json;

namespace OVB.Demos.Eschody.WebApi.Controllers.StudentContext;

[Route("api/v1/backoffice/student")]
[ApiController]
public sealed class StudentController : CustomControllerBase
{
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [Route("create")]
    [AllowAnonymous]
    public async Task<IActionResult> HttpPostCreateStudentServiceAsync(
        [FromServices] IUseCase<CreateStudentUseCaseInput, CreateStudentUseCaseResult> useCase,
        [FromServices] ICacheRepository cacheRepository,
        [FromHeader(Name = AuditableInfoValueObject.IdempotencyKey)] string idempotencyKey,
        [FromHeader(Name = AuditableInfoValueObject.CorrelationIdKey)] Guid correlationId,
        [FromHeader(Name = AuditableInfoValueObject.SourcePlatformKey)] string sourcePlatform,
        [FromHeader(Name = AuditableInfoValueObject.ExecutionUserKey)] string executionUser,
        [FromBody] CreateStudentPayloadInput input,
        CancellationToken cancellationToken)
    {
        var cache = await cacheRepository.GetCacheAsync(
            key: $"{nameof(HttpPostCreateStudentServiceAsync)}.{idempotencyKey}",
            cancellationToken: cancellationToken);
        if (cache is not null)
        {
            var memoryStream = new MemoryStream(cache);
            var cacheModel = await JsonSerializer.DeserializeAsync<CacheRequestModel>(
                utf8Json: memoryStream,
                cancellationToken: cancellationToken);
            return StatusCode(
                statusCode: cacheModel!.StatusCode,
                value: cacheModel.Response);
        }

        var auditableInfo = AuditableInfoValueObject.Build(
            correlationId: correlationId,
            sourcePlatform: sourcePlatform,
            executionUser: executionUser,
            requestedAt: DateTime.UtcNow);
        if (!auditableInfo.IsValid)
            return StatusCode(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                value: GetUnprocessableEntityForInvalidAuditable());

        HttpContext.Response.Headers.Append(AuditableInfoValueObject.CorrelationIdKey, auditableInfo.GetCorrelationId().ToString());
        HttpContext.Response.Headers.Append(AuditableInfoValueObject.RequestedAtKey, auditableInfo.GetRequestedAt().ToString("dd/MM/yyyy HH:mm:ss"));
        HttpContext.Response.Headers.Append(AuditableInfoValueObject.SourcePlatformKey, auditableInfo.GetSourcePlatform());
        HttpContext.Response.Headers.Append(AuditableInfoValueObject.ExecutionUserKey, auditableInfo.GetExecutionUser());
        HttpContext.Response.Headers.Append(AuditableInfoValueObject.IdempotencyKey, idempotencyKey);

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

        if (useCaseResult.IsSuccess)
        {
            var memoryStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(
                    utf8Json: memoryStream,
                    value: new CacheRequestModel(
                        statusCode: StatusCodes.Status201Created,
                        response: useCaseResult.Output),
                    cancellationToken: cancellationToken);
            await cacheRepository.SetCacheAsync(
                key: $"{nameof(HttpPostCreateStudentServiceAsync)}.{idempotencyKey}",
                value: memoryStream.ToArray(),
                expirationSeconds: 86400,
                memoryExpirationSeconds: 300,
                cancellationToken: cancellationToken);

            return StatusCode(
                statusCode: StatusCodes.Status201Created,
                value: useCaseResult.Output);
        }

        if (useCaseResult.IsPartial)
            return StatusCode(StatusCodes.Status503ServiceUnavailable, null);

        return StatusCode(
            statusCode: StatusCodes.Status400BadRequest, 
            value: useCaseResult.Notifications);
    }
}
