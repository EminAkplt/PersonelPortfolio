using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Common.Caching;
using Portfolio.Web.Data;
using Portfolio.Web.Domain;
using Portfolio.Web.Features.Projects;

namespace Portfolio.Web.Features.Admin.Projects.SaveProject;

public sealed class SaveProjectHandler(
    AppDbContext db,
    IValidator<SaveProjectRequest> validator,
    IAppCache cache)
{
    public async Task<Result<int>> HandleAsync(SaveProjectRequest request, CancellationToken ct = default)
    {
        var validation = await validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return Result.Fail<int>(Error.Validation("project_invalid", validation.Errors[0].ErrorMessage));

        var slugTaken = await db.Projects
            .AnyAsync(p => p.Slug == request.Slug && p.Id != (request.Id ?? 0), ct);
        if (slugTaken)
            return Result.Fail<int>(Error.Conflict("slug_taken", "Bu slug başka bir projede kullanılıyor."));

        Project project;
        var now = DateTimeOffset.UtcNow;

        if (request.Id is > 0)
        {
            var existing = await db.Projects.FindAsync([request.Id.Value], ct);
            if (existing is null)
                return Result.Fail<int>(Error.NotFound("project_not_found", "Düzenlenmek istenen proje bulunamadı."));
            project = existing;
        }
        else
        {
            project = new Project
            {
                Slug = request.Slug!,
                Title = request.Title!,
                Summary = request.Summary!,
                Problem = request.Problem!,
                Approach = request.Approach!,
                Outcome = request.Outcome!,
                CreatedAt = now
            };
            db.Projects.Add(project);
        }

        var oldSlug = project.Slug;

        project.Slug = request.Slug!;
        project.Title = request.Title!;
        project.Summary = request.Summary!;
        project.Problem = request.Problem!;
        project.Approach = request.Approach!;
        project.Outcome = request.Outcome!;
        project.TitleEn = NullIfEmpty(request.TitleEn);
        project.SummaryEn = NullIfEmpty(request.SummaryEn);
        project.ProblemEn = NullIfEmpty(request.ProblemEn);
        project.ApproachEn = NullIfEmpty(request.ApproachEn);
        project.OutcomeEn = NullIfEmpty(request.OutcomeEn);
        project.TechStack = request.TechStack;
        project.CoverImageUrl = NullIfEmpty(request.CoverImageUrl);
        project.DemoUrl = NullIfEmpty(request.DemoUrl);
        project.RepoUrl = NullIfEmpty(request.RepoUrl);
        project.DisplayOrder = request.DisplayOrder;
        project.IsPublished = request.IsPublished;
        project.UpdatedAt = now;

        await db.SaveChangesAsync(ct);

        ProjectCache.InvalidateAll(cache, oldSlug, project.Slug);

        return Result.Success(project.Id);
    }

    private static string? NullIfEmpty(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
