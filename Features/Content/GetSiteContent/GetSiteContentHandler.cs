using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Common.Caching;
using Portfolio.Web.Data;

namespace Portfolio.Web.Features.Content.GetSiteContent;

public sealed class GetSiteContentHandler(AppDbContext db, IAppCache cache)
{
    public const string CacheKey = "content:all";
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public async Task<IReadOnlyDictionary<string, string>> HandleAsync(CancellationToken ct = default)
    {
        var contents = await cache.GetOrCreateAsync<Dictionary<string, string>>(CacheKey, CacheTtl, async token =>
            await db.SiteContents
                .AsNoTracking()
                .ToDictionaryAsync(c => c.Key, c => c.Value, token), ct);

        return contents ?? [];
    }
}
