using System.Text.Json;

namespace OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Models;

public sealed record CacheRequestModel
{
    private CacheRequestModel(int statusCode, string response)
    {
        StatusCode = statusCode;
        Response = response;
    }

    public int StatusCode { get; }
    public string Response { get; }

    public static CacheRequestModel Build(int statusCode, string response)
        => new CacheRequestModel(statusCode, response);

    public string Serialize()
        => JsonSerializer.Serialize(this);
}
