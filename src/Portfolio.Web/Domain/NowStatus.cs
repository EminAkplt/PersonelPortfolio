namespace Portfolio.Web.Domain;

public enum Mood
{
    Building,
    Learning,
    Shipping
}

/// <summary>"Şu an" widget'ının tek satırlık kaynağı.</summary>
public class NowStatus
{
    public int Id { get; set; }
    public required string StatusText { get; set; }
    public Mood Mood { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
