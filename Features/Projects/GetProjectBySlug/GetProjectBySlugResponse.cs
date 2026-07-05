namespace Portfolio.Web.Features.Projects.GetProjectBySlug;

public sealed record ProjectDetail(
    string Slug,
    string Title,
    string Summary,
    string Problem,
    string Approach,
    string Outcome,
    string[] TechStack,
    string? CoverImageUrl,
    string? DemoUrl,
    string? RepoUrl,
    DateTimeOffset UpdatedAt);
