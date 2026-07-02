using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Common.Caching;
using Portfolio.Web.Data;

namespace Portfolio.Web.Features.Projects.GetProjectBySlug;

public sealed class GetProjectBySlugHandler(AppDbContext db, IAppCache cache)
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public static string CacheKey(string slug) => $"projects:detail:{slug}";

    public async Task<Result<ProjectDetail>> HandleAsync(string slug, CancellationToken ct = default)
    {
        var project = await cache.GetOrCreateAsync(CacheKey(slug), CacheTtl, async token =>
            await db.Projects
                .AsNoTracking()
                .Where(p => p.IsPublished && p.Slug == slug)
                .Select(p => new ProjectDetail(
                    p.Slug, p.Title, p.Summary, p.Problem, p.Approach, p.Outcome,
                    p.TechStack, p.CoverImageUrl, p.DemoUrl, p.RepoUrl, p.UpdatedAt))
                .FirstOrDefaultAsync(token), ct);

        return project is null
            ? Result.Fail<ProjectDetail>(Error.NotFound("project_not_found", "Aradığınız proje bulunamadı."))
            : Result.Success(project);
    }
}
