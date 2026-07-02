using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Common.Caching;
using Portfolio.Web.Data;

namespace Portfolio.Web.Features.Projects.GetProjects;

public sealed class GetProjectsHandler(AppDbContext db, IAppCache cache)
{
    public const string CacheKey = "projects:published";
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public async Task<Result<IReadOnlyList<ProjectListItem>>> HandleAsync(CancellationToken ct = default)
    {
        var projects = await cache.GetOrCreateAsync<IReadOnlyList<ProjectListItem>>(CacheKey, CacheTtl, async token =>
            await db.Projects
                .AsNoTracking()
                .Where(p => p.IsPublished)
                .OrderBy(p => p.DisplayOrder)
                .Select(p => new ProjectListItem(p.Slug, p.Title, p.Summary, p.TechStack, p.CoverImageUrl))
                .ToListAsync(token), ct);

        return Result.Success(projects ?? []);
    }
}
