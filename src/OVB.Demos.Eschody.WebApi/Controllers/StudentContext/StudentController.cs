using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OVB.Demos.Eschody.Application.UseCases.Interfaces;
using OVB.Demos.Eschody.Application.UseCases.StudentContext.CreateStudent.Inputs;
using OVB.Demos.Eschody.Application.UseCases.StudentContext.CreateStudent.Outputs;
using OVB.Demos.Eschody.Domain.StudentContext.ENUMs;
using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Interfaces;
using OVB.Demos.Eschody.Libraries.Observability.Metric.Interfaces;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Facilitators;
using OVB.Demos.Eschody.Libraries.Observability.Trace.Interfaces;
using OVB.Demos.Eschody.Libraries.ValueObjects;
using OVB.Demos.Eschody.WebApi.Controllers.Base;
using OVB.Demos.Eschody.WebApi.Controllers.StudentContext.Payloads;
using System.Diagnostics;
using System.Net.Mime;
using System.Text.Json;

namespace OVB.Demos.Eschody.WebApi.Controllers.StudentContext;

[Route("api/v1/backoffice/student")]
public sealed class StudentController : CustomControllerBase
{
    public StudentController(
        ITraceManager traceManager,
        ICacheRepository cacheRepository,
        IMetricManager metricManager) : base(traceManager, cacheRepository, metricManager){}


    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [Route("create")]
    [Authorize(Roles = "Tenant")]
    public Task<IActionResult> HttpPostCreateStudentServiceAsync(
        [FromServices] IUseCase<CreateStudentUseCaseInput, CreateStudentUseCaseResult> useCase,
        [FromHeader(Name = AuditableInfoValueObject.IdempotencyHeaderKey)] string idempotencyKey,
        [FromHeader(Name = AuditableInfoValueObject.CorrelationIdKey)] Guid correlationId,
        [FromHeader(Name = AuditableInfoValueObject.SourcePlatformKey)] string sourcePlatform,
        [FromHeader(Name = AuditableInfoValueObject.ExecutionUserKey)] string executionUser,
        [FromBody] CreateStudentPayloadInput input,
        CancellationToken cancellationToken)
    {
        var auditableInfo = AuditableInfoValueObject.Build(
            correlationId: correlationId,
            sourcePlatform: sourcePlatform,
            executionUser: executionUser,
            idempotencyKey: idempotencyKey,
            requestedAt: DateTime.UtcNow);
        if (!auditableInfo.IsValid)
            return Task.FromResult((IActionResult)StatusCode(
                statusCode: StatusCodes.Status422UnprocessableEntity,
                value: GetUnprocessableEntityForInvalidAuditable()));

        return _traceManager.ExecuteTraceAsync<CreateStudentPayloadInput, IActionResult>(
            traceName: nameof(HttpPostCreateStudentServiceAsync),
            activityKind: ActivityKind.Internal,
            input: input,
            handler: async (inputTrace, inputAuditableInfo, inputActivity, inputCancellationToken) =>
            {
                var actionCacheKey = inputAuditableInfo.GenerateCacheKeyWithIdempotencyKey(
                        cacheKey: nameof(HttpPostCreateStudentServiceAsync));
                var statusCode = 503;
                var hasUsedIdempotencyCache = false;

                HttpContext.Response.Headers.Append(AuditableInfoValueObject.CorrelationIdKey, inputAuditableInfo.GetCorrelationId().ToString());
                HttpContext.Response.Headers.Append(AuditableInfoValueObject.RequestedAtKey, inputAuditableInfo.GetRequestedAt().ToString("dd/MM/yyyy HH:mm:ss"));
                HttpContext.Response.Headers.Append(AuditableInfoValueObject.SourcePlatformKey, inputAuditableInfo.GetSourcePlatform());
                HttpContext.Response.Headers.Append(AuditableInfoValueObject.ExecutionUserKey, inputAuditableInfo.GetExecutionUser());
                HttpContext.Response.Headers.Append(AuditableInfoValueObject.IdempotencyHeaderKey, inputAuditableInfo.GetIdempotencyKey());

                _metricManager.CreateCounterIfNotExists(
                    counterName: nameof(HttpPostCreateStudentServiceAsync));
                _metricManager.IncrementCounter(
                    counterName: nameof(HttpPostCreateStudentServiceAsync),
                    auditableInfo: inputAuditableInfo,
                    keyValuePairs: [
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.CorrelationIdKey,
                            value: (object?)inputAuditableInfo.GetCorrelationId().ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.SourcePlatformKey,
                            value: (object?)inputAuditableInfo.GetSourcePlatform().ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.ExecutionUserKey,
                            value: (object?)inputAuditableInfo.GetExecutionUser().ToString())
                    ]);

                var cache = await GetCacheFromIdempotencyKeyAsync(
                    actionCacheKey: actionCacheKey,
                    auditableInfo: inputAuditableInfo,
                    cancellationToken: inputCancellationToken);
                if (cache is not null)
                {
                    statusCode = cache.StatusCode;
                    hasUsedIdempotencyCache = true;

                    inputActivity.AppendSpanTag(
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HttpMethodKey,
                            value: "HTTP POST"),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.EndpointKey,
                            value: "api/v1/backoffice/student/create"),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.CorrelationIdKey,
                            value: inputAuditableInfo.GetCorrelationId().ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.ExecutionUserKey,
                            value: inputAuditableInfo.GetExecutionUser()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.SourcePlatformKey,
                            value: inputAuditableInfo.GetSourcePlatform()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.IdempotencyKey,
                            value: inputAuditableInfo.GetIdempotencyKey()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.StatusCodeKey,
                            value: statusCode.ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HasUsedIdempotencyCache,
                            value: hasUsedIdempotencyCache.ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.RemoteHostKey,
                            value: HttpContext.Request.Headers["REMOTE_HOST"].ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.RemoteAddrKey,
                            value: HttpContext.Request.Headers["REMOTE_ADDR"].ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HttpForwardedForKey,
                            value: HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].ToString()));

                    return StatusCode(
                        statusCode: cache.StatusCode,
                        value: cache.Response);
                }

                var useCaseResult = await useCase.ExecuteUseCaseAsync(
                    input: CreateStudentUseCaseInput.Build(
                        firstName: FirstNameValueObject.Build(inputTrace.FirstName),
                        lastName: LastNameValueObject.Build(inputTrace.LastName),
                        email: EmailValueObject.Build(inputTrace.Email),
                        phone: PhoneValueObject.Build(inputTrace.Phone),
                        password: PasswordValueObject.Build(inputTrace.Password, false),
                        typeStudent: (TypeStudent)inputTrace.TypeStudent),
                    auditableInfo: inputAuditableInfo,
                    cancellationToken: inputCancellationToken);

                if (useCaseResult.IsSuccess)
                {
                    statusCode = StatusCodes.Status201Created;
                    await SetCacheFromIdempotencyKeyAsync(
                        actionCacheKey: actionCacheKey,
                        statusCode: statusCode,
                        content: useCaseResult.Output,
                        auditableInfo: inputAuditableInfo,
                        cancellationToken: cancellationToken);

                    inputActivity.AppendSpanTag(
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HttpMethodKey,
                            value: "HTTP POST"),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.EndpointKey,
                            value: "api/v1/backoffice/student/create"),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.CorrelationIdKey,
                            value: inputAuditableInfo.GetCorrelationId().ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.ExecutionUserKey,
                            value: inputAuditableInfo.GetExecutionUser()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.SourcePlatformKey,
                            value: inputAuditableInfo.GetSourcePlatform()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.IdempotencyKey,
                            value: inputAuditableInfo.GetIdempotencyKey()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.StatusCodeKey,
                            value: statusCode.ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HasUsedIdempotencyCache,
                            value: hasUsedIdempotencyCache.ToString()),
                        KeyValuePair.Create(
                            key: nameof(CreateStudentUseCaseResult.StudentId),
                            value: useCaseResult.Output.StudentId.ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.RemoteHostKey,
                            value: HttpContext.Request.Headers["REMOTE_HOST"].ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.RemoteAddrKey,
                            value: HttpContext.Request.Headers["REMOTE_ADDR"].ToString()),
                        KeyValuePair.Create(
                            key: ObservabilityFacilitator.HttpForwardedForKey,
                            value: HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].ToString()));

                    return StatusCode(
                        statusCode: statusCode,
                        value: useCaseResult.Output);
                }

                if (useCaseResult.IsPartial)
                    throw new NotImplementedException();

                inputActivity.AppendSpanTag(
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.HttpMethodKey,
                        value: "HTTP POST"),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.EndpointKey,
                        value: "api/v1/backoffice/student/create"),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.CorrelationIdKey,
                        value: inputAuditableInfo.GetCorrelationId().ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.ExecutionUserKey,
                        value: inputAuditableInfo.GetExecutionUser()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.SourcePlatformKey,
                        value: inputAuditableInfo.GetSourcePlatform()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.IdempotencyKey,
                        value: inputAuditableInfo.GetIdempotencyKey()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.StatusCodeKey,
                        value: statusCode.ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.HasUsedIdempotencyCache,
                        value: hasUsedIdempotencyCache.ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.RemoteHostKey,
                        value: HttpContext.Request.Headers["REMOTE_HOST"].ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.RemoteAddrKey,
                        value: HttpContext.Request.Headers["REMOTE_ADDR"].ToString()),
                    KeyValuePair.Create(
                        key: ObservabilityFacilitator.HttpForwardedForKey,
                        value: HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].ToString()));

                statusCode = StatusCodes.Status400BadRequest;

                await SetCacheFromIdempotencyKeyAsync(
                    actionCacheKey: actionCacheKey,
                    statusCode: statusCode,
                    content: useCaseResult.Notifications,
                    auditableInfo: inputAuditableInfo,
                    cancellationToken: cancellationToken);

                return StatusCode(
                    statusCode: statusCode,
                    value: useCaseResult.Notifications);
            },
            auditableInfo: auditableInfo,
            cancellationToken: cancellationToken);
    }
}
