﻿using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Interfaces;

public interface ICacheRepository
{
    public Task<byte[]?> GetCacheAsync(string key, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken);
    public Task SetCacheAsync(string key, byte[] value, AuditableInfoValueObject auditableInfo, CancellationToken cancellationToken);
}
