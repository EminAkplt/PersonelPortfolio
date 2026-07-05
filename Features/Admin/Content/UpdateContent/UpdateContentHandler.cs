using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Common.Caching;
using Portfolio.Web.Data;
using Portfolio.Web.Domain;
using Portfolio.Web.Features.Content.GetSiteContent;

namespace Portfolio.Web.Features.Admin.Content.UpdateContent;

public sealed class UpdateContentHandler(AppDbContext db, IAppCache cache)
{
    public async Task<Result> HandleAsync(IReadOnlyDictionary<string, string> values, CancellationToken ct = default)
    {
        var unknownKey = values.Keys.FirstOrDefault(k => !SiteContentKeys.Writable.Contains(k));
        if (unknownKey is not null)
            return Result.Fail(Error.Validation("unknown_content_key", $"Bilinmeyen içerik anahtarı: {unknownKey}"));

        var now = DateTimeOffset.UtcNow;
        var existing = await db.SiteContents
            .Where(c => values.Keys.Contains(c.Key))
            .ToDictionaryAsync(c => c.Key, ct);

        foreach (var (key, value) in values)
        {
            if (existing.TryGetValue(key, out var content))
            {
                if (content.Value == value)
                    continue;
                content.Value = value;
                content.UpdatedAt = now;
            }
            else
            {
                db.SiteContents.Add(new SiteContent { Key = key, Value = value, UpdatedAt = now });
            }
        }

        await db.SaveChangesAsync(ct);
        cache.Remove(GetSiteContentHandler.CacheKey);
        return Result.Success();
    }
}
