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
    // Dilden bağımsız (tek değer)
    public const string HeroName = "hero.name";
    public const string ContactEmail = "contact.email";
    public const string SocialLinkedin = "social.linkedin";
    public const string SocialGithub = "social.github";
    public const string CvFile = "cv.file";

    // Yerelleştirilebilir (temel = TR, ".en" varyantı EN)
    public const string HeroTagline = "hero.tagline";
    public const string ConsoleText = "console.text";
    public const string AboutText = "about.text";
    public const string Principle1Title = "principle.1.title";
    public const string Principle1Text = "principle.1.text";
    public const string Principle2Title = "principle.2.title";
    public const string Principle2Text = "principle.2.text";
    public const string Principle3Title = "principle.3.title";
    public const string Principle3Text = "principle.3.text";

    /// <summary>EN çevirisi olan anahtarlar — admin bunları çift dilli düzenler.</summary>
    public static readonly string[] Localizable =
    [
        HeroTagline, ConsoleText, AboutText,
        Principle1Title, Principle1Text,
        Principle2Title, Principle2Text,
        Principle3Title, Principle3Text
    ];

    /// <summary>Tek değerli (dilden bağımsız) anahtarlar.</summary>
    public static readonly string[] Neutral =
    [
        HeroName, ContactEmail, SocialLinkedin, SocialGithub, CvFile
    ];

    /// <summary>Admin'in yazabileceği tüm geçerli anahtarlar (temel + ".en" varyantları).</summary>
    public static readonly HashSet<string> Writable =
    [
        .. Neutral,
        .. Localizable,
        .. Localizable.Select(k => $"{k}.en")
    ];
}
