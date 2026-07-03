using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Common.Caching;
using Portfolio.Web.Common.Localization;
using Portfolio.Web.Data;

namespace Portfolio.Web.Features.Projects.GetProjectBySlug;

public sealed class GetProjectBySlugHandler(AppDbContext db, IAppCache cache)
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public static string CacheKey(string slug, string culture) => $"projects:detail:{culture}:{slug}";

    public async Task<Result<ProjectDetail>> HandleAsync(string slug, CancellationToken ct = default)
    {
        var culture = Loc.Culture;

        var project = await cache.GetOrCreateAsync(CacheKey(slug, culture), CacheTtl, async token =>
        {
            var p = await db.Projects
                .AsNoTracking()
                .Where(x => x.IsPublished && x.Slug == slug)
                .FirstOrDefaultAsync(token);

            return p is null
                ? null
                : new ProjectDetail(
                    p.Slug,
                    Loc.Pick(p.Title, p.TitleEn),
                    Loc.Pick(p.Summary, p.SummaryEn),
                    Loc.Pick(p.Problem, p.ProblemEn),
                    Loc.Pick(p.Approach, p.ApproachEn),
                    Loc.Pick(p.Outcome, p.OutcomeEn),
                    p.TechStack, p.CoverImageUrl, p.DemoUrl, p.RepoUrl, p.UpdatedAt);
        }, ct);

        return project is null
            ? Result.Fail<ProjectDetail>(Error.NotFound("project_not_found", "Aradığınız proje bulunamadı."))
            : Result.Success(project);
    }
}
