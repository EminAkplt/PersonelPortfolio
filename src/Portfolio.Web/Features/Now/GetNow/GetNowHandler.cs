using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Common.Caching;
using Portfolio.Web.Data;

namespace Portfolio.Web.Features.Now.GetNow;

public sealed class GetNowHandler(AppDbContext db, IAppCache cache)
{
    public const string CacheKey = "now:status";
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(1);

    public async Task<Result<NowResponse>> HandleAsync(CancellationToken ct = default)
    {
        var status = await cache.GetOrCreateAsync(CacheKey, CacheTtl, async token =>
            await db.NowStatuses
                .AsNoTracking()
                .OrderByDescending(s => s.UpdatedAt)
                .Select(s => new NowResponse(s.StatusText, s.Mood.ToString(), s.UpdatedAt))
                .FirstOrDefaultAsync(token), ct);

        return status is null
            ? Result.Fail<NowResponse>(Error.NotFound("now_not_set", "Şu an durumu henüz ayarlanmamış."))
            : Result.Success(status);
    }
}
