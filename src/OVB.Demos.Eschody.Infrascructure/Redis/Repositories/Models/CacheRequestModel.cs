using System.Text.Json;

namespace OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Models;

public sealed record CacheRequestModel
{
    public CacheRequestModel(int statusCode, object? response)
    {
        StatusCode = statusCode;
        Response = response;
    }

    public int StatusCode { get; init; }
    public object? Response { get; init; }
}
