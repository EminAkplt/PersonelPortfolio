namespace Portfolio.Web.Domain;

public class Project
{
    public int Id { get; set; }
    public required string Slug { get; set; }
    public required string Title { get; set; }

    /// <summary>Tek cümle, insan dilinde özet.</summary>
    public required string Summary { get; set; }

    public required string Problem { get; set; }
    public required string Approach { get; set; }
    public required string Outcome { get; set; }

    public string[] TechStack { get; set; } = [];
    public string? CoverImageUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string? RepoUrl { get; set; }

    public int DisplayOrder { get; set; }
    public bool IsPublished { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
