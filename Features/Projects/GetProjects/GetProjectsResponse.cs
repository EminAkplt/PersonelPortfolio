namespace Portfolio.Web.Features.Projects.GetProjects;

public sealed record ProjectListItem(
    string Slug,
    string Title,
    string Summary,
    string[] TechStack,
    string? CoverImageUrl);
