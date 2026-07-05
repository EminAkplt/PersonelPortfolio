using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Common.Caching;
using Portfolio.Web.Data;
using Portfolio.Web.Domain;
using Portfolio.Web.Features.Now.GetNow;

namespace Portfolio.Web.Features.Admin.Now.UpdateNow;

public sealed class UpdateNowHandler(AppDbContext db, IAppCache cache)
{
    public async Task<Result> HandleAsync(string? statusText, Mood mood, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(statusText))
            return Result.Fail(Error.Validation("status_empty", "Durum metni boş olamaz."));
        if (statusText.Length > 300)
            return Result.Fail(Error.Validation("status_too_long", "Durum metni 300 karakteri aşamaz."));

        var status = await db.NowStatuses.OrderByDescending(s => s.UpdatedAt).FirstOrDefaultAsync(ct);
        if (status is null)
        {
            status = new NowStatus { StatusText = statusText.Trim() };
            db.NowStatuses.Add(status);
        }

        status.StatusText = statusText.Trim();
        status.Mood = mood;
        status.UpdatedAt = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync(ct);
        cache.Remove(GetNowHandler.CacheKey);
        return Result.Success();
    }
}
