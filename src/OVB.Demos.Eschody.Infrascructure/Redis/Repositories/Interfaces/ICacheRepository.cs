namespace OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Interfaces;

public interface ICacheRepository
{
    public Task<byte[]?> GetCacheAsync(string key, CancellationToken cancellationToken);
    public Task SetCacheAsync(string key, byte[] value, int expirationSeconds, int memoryExpirationSeconds, CancellationToken cancellationToken);
}
