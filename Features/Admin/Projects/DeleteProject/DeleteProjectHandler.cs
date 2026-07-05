using Portfolio.Web.Common.Caching;
using Portfolio.Web.Data;
using Portfolio.Web.Features.Projects;

namespace Portfolio.Web.Features.Admin.Projects.DeleteProject;

public sealed class DeleteProjectHandler(AppDbContext db, IAppCache cache)
{
    public async Task<Result> HandleAsync(int id, CancellationToken ct = default)
    {
        var project = await db.Projects.FindAsync([id], ct);
        if (project is null)
            return Result.Fail(Error.NotFound("project_not_found", "Silinmek istenen proje bulunamadı."));

        db.Projects.Remove(project);
        await db.SaveChangesAsync(ct);

        ProjectCache.InvalidateAll(cache, project.Slug);

        return Result.Success();
    }
}
