using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;

namespace Portfolio.Web.Features.Admin.Dashboard.GetDashboard;

public sealed record DailyViews(DateOnly Day, int Count);

public sealed record DashboardData(
    IReadOnlyList<DailyViews> Last30Days,
    int TotalViewsLast30Days,
    int UnreadMessageCount,
    int PublishedProjectCount);

public sealed class GetDashboardHandler(AppDbContext db)
{
    public async Task<Result<DashboardData>> HandleAsync(CancellationToken ct = default)
    {
        var since = DateTimeOffset.UtcNow.AddDays(-29).Date;
        var sinceOffset = new DateTimeOffset(since, TimeSpan.Zero);

        var rawCounts = await db.PageViews
            .Where(v => v.VisitedAt >= sinceOffset)
            .GroupBy(v => v.VisitedAt.Date)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToListAsync(ct);

        var byDay = rawCounts.ToDictionary(x => DateOnly.FromDateTime(x.Key), x => x.Count);

        var series = Enumerable.Range(0, 30)
            .Select(i => DateOnly.FromDateTime(since.AddDays(i)))
            .Select(day => new DailyViews(day, byDay.GetValueOrDefault(day)))
            .ToList();

        var unread = await db.ContactMessages.CountAsync(m => !m.IsRead, ct);
        var published = await db.Projects.CountAsync(p => p.IsPublished, ct);

        return Result.Success(new DashboardData(series, series.Sum(d => d.Count), unread, published));
    }
}
