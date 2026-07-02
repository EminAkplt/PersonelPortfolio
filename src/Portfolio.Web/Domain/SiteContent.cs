namespace Portfolio.Web.Domain;

/// <summary>Hero cümlesi, hakkımda metni gibi admin'den düzenlenebilir içerikler.</summary>
public class SiteContent
{
    public required string Key { get; set; }
    public required string Value { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public static class SiteContentKeys
{
    public const string HeroName = "hero.name";
    public const string HeroTagline = "hero.tagline";
    public const string AboutText = "about.text";
    public const string Principle1Title = "principle.1.title";
    public const string Principle1Text = "principle.1.text";
    public const string Principle2Title = "principle.2.title";
    public const string Principle2Text = "principle.2.text";
    public const string Principle3Title = "principle.3.title";
    public const string Principle3Text = "principle.3.text";
    public const string ContactEmail = "contact.email";

    public static readonly string[] All =
    [
        HeroName, HeroTagline, AboutText,
        Principle1Title, Principle1Text,
        Principle2Title, Principle2Text,
        Principle3Title, Principle3Text,
        ContactEmail
    ];
}
