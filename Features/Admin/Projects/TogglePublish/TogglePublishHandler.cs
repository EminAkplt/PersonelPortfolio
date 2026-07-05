using Portfolio.Web.Common.Caching;
using Portfolio.Web.Data;
using Portfolio.Web.Features.Projects;

namespace Portfolio.Web.Features.Admin.Projects.TogglePublish;

public sealed class TogglePublishHandler(AppDbContext db, IAppCache cache)
{
    public async Task<Result<bool>> HandleAsync(int id, CancellationToken ct = default)
    {
        var project = await db.Projects.FindAsync([id], ct);
        if (project is null)
            return Result.Fail<bool>(Error.NotFound("project_not_found", "Proje bulunamadı."));

        project.IsPublished = !project.IsPublished;
        project.UpdatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);

        ProjectCache.InvalidateAll(cache, project.Slug);

        return Result.Success(project.IsPublished);
    }
}
