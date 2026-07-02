namespace Portfolio.Web.Features.Admin.Projects.SaveProject;

public sealed record SaveProjectRequest(
    int? Id,
    string? Slug,
    string? Title,
    string? Summary,
    string? Problem,
    string? Approach,
    string? Outcome,
    string[] TechStack,
    string? CoverImageUrl,
    string? DemoUrl,
    string? RepoUrl,
    int DisplayOrder,
    bool IsPublished);
