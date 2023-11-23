using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using OVB.Demos.Eschody.WebApi;
using System.Net.Http.Json;

namespace OVB.Demos.Eschody.IntegrationTests.StudentContext;

public sealed class CreateStudentTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CreateStudentTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("api/v1/backoffice/students")]
    public async Task HttpPost_ReturnServiceUnavailable(string url)
    {
        // Arrange
        using var httpClient = _factory.CreateClient();
        httpClient.DefaultRequestHeaders.Add(
            name: "X-Idempotency-Key",
            value: "testing-idempotency-key-1");
        httpClient.DefaultRequestHeaders.Add(
            name: "X-Correlation-Id",
            value: Guid.NewGuid().ToString());
        httpClient.DefaultRequestHeaders.Add(
            name: "X-Source-Platform",
            value: "aspnet/integration/tests");
        httpClient.DefaultRequestHeaders.Add(
            name: "X-Execution-User",
            value: "user-tester");

        // Act
        var result = await httpClient.PostAsJsonAsync(
            requestUri: $"{httpClient.BaseAddress}{url}",
            value: string.Empty, 
            cancellationToken: default);

        // Assert
        Assert.Equal(
            expected: StatusCodes.Status503ServiceUnavailable,
            actual: (int)result.StatusCode);
    }
}
