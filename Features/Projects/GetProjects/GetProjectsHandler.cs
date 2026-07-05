using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Common.Caching;
using Portfolio.Web.Common.Localization;
using Portfolio.Web.Data;

namespace Portfolio.Web.Features.Projects.GetProjects;

public sealed class GetProjectsHandler(AppDbContext db, IAppCache cache)
{
    public const string CacheKeyBase = "projects:published";
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public static string CacheKey(string culture) => $"{CacheKeyBase}:{culture}";

    /// <summary>Yayınlanmış projeleri geçerli kültüre göre çözümlenmiş metinlerle döndürür.</summary>
    public async Task<Result<IReadOnlyList<ProjectListItem>>> HandleAsync(CancellationToken ct = default)
    {
        var culture = Loc.Culture;

        var projects = await cache.GetOrCreateAsync<IReadOnlyList<ProjectListItem>>(CacheKey(culture), CacheTtl, async token =>
        {
            var rows = await db.Projects
                .AsNoTracking()
                .Where(p => p.IsPublished)
                .OrderBy(p => p.DisplayOrder)
                .Select(p => new { p.Slug, p.Title, p.TitleEn, p.Summary, p.SummaryEn, p.TechStack, p.CoverImageUrl })
                .ToListAsync(token);

            return rows
                .Select(r => new ProjectListItem(
                    r.Slug,
                    Loc.Pick(r.Title, r.TitleEn),
                    Loc.Pick(r.Summary, r.SummaryEn),
                    r.TechStack,
                    r.CoverImageUrl))
                .ToList();
        }, ct);

        return Result.Success(projects ?? []);
    }
}
