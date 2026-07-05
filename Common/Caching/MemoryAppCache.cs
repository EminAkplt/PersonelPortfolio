using Microsoft.Extensions.Caching.Memory;

namespace Portfolio.Web.Common.Caching;

public sealed class MemoryAppCache(IMemoryCache cache) : IAppCache
{
    public async ValueTask<T?> GetOrCreateAsync<T>(string key, TimeSpan ttl, Func<CancellationToken, ValueTask<T?>> factory, CancellationToken ct = default)
    {
        if (cache.TryGetValue(key, out T? cached))
            return cached;

        var value = await factory(ct);
        if (value is not null)
            cache.Set(key, value, ttl);

        return value;
    }

    public void Remove(string key) => cache.Remove(key);
}
