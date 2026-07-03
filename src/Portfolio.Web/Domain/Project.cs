namespace Portfolio.Web.Domain;

public class Project
{
    public int Id { get; set; }
    public required string Slug { get; set; }

    // Temel dil (TR)
    public required string Title { get; set; }

    /// <summary>Tek cümle, insan dilinde özet.</summary>
    public required string Summary { get; set; }

    public required string Problem { get; set; }
    public required string Approach { get; set; }
    public required string Outcome { get; set; }

    // İngilizce çeviriler (opsiyonel — boşsa TR'ye düşülür)
    public string? TitleEn { get; set; }
    public string? SummaryEn { get; set; }
    public string? ProblemEn { get; set; }
    public string? ApproachEn { get; set; }
    public string? OutcomeEn { get; set; }

    public string[] TechStack { get; set; } = [];
    public string? CoverImageUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string? RepoUrl { get; set; }

    public int DisplayOrder { get; set; }
    public bool IsPublished { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
