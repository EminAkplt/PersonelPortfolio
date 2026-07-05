namespace Portfolio.Web.Common.Caching;

/// <summary>
/// İnce cache soyutlaması. Bugün in-memory çalışır; Redis'e geçişte
/// yalnızca bu arayüzün yeni bir implementasyonu kaydedilir.
/// </summary>
public interface IAppCache
{
    ValueTask<T?> GetOrCreateAsync<T>(string key, TimeSpan ttl, Func<CancellationToken, ValueTask<T?>> factory, CancellationToken ct = default);
    void Remove(string key);
}
