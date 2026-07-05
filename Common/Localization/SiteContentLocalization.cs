namespace Portfolio.Web.Common.Localization;

public static class SiteContentLocalization
{
    /// <summary>
    /// SiteContent sözlüğünden geçerli kültüre uygun değeri seçer.
    /// EN için "key.en" anahtarına bakar; yoksa TR (temel) anahtara düşer.
    /// </summary>
    public static string Localized(this IReadOnlyDictionary<string, string> content, string key, string fallback = "")
    {
        var culture = Loc.Culture;
        if (culture != Loc.DefaultCulture
            && content.TryGetValue($"{key}.{culture}", out var localized)
            && !string.IsNullOrWhiteSpace(localized))
        {
            return localized;
        }

        return content.GetValueOrDefault(key, fallback);
    }
}
