using System.Globalization;

namespace Portfolio.Web.Common.Localization;

/// <summary>
/// Hafif, framework'süz yerelleştirme. UI metinleri anahtar-değer olarak
/// tr/en sözlüklerinde tutulur; geçerli kültür istek ortamından okunur.
/// DB içeriği (proje metinleri, hakkımda) ayrı ele alınır.
/// </summary>
public static class Loc
{
    public const string DefaultCulture = "tr";
    public static readonly IReadOnlyList<string> Supported = ["tr", "en"];

    /// <summary>Geçerli isteğin iki harfli dil kodu ("tr" / "en").</summary>
    public static string Culture
    {
        get
        {
            var c = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return Supported.Contains(c) ? c : DefaultCulture;
        }
    }

    public static bool IsSupported(string? culture) =>
        culture is not null && Supported.Contains(culture);

    /// <summary>İki dilli bir DB alanından geçerli kültüre uygun olanı seçer (EN boşsa TR'ye düşer).</summary>
    public static string Pick(string tr, string? en) =>
        Culture == "en" && !string.IsNullOrWhiteSpace(en) ? en! : tr;

    /// <summary>UI metnini geçerli kültüre göre döndürür; bulunamazsa anahtarı verir.</summary>
    public static string T(string key)
    {
        var culture = Culture;
        if (Strings.TryGetValue(culture, out var table) && table.TryGetValue(key, out var value))
            return value;
        if (Strings[DefaultCulture].TryGetValue(key, out var fallback))
            return fallback;
        return key;
    }

    public static string CultureLabel(string culture) => culture switch
    {
        "tr" => "Türkçe",
        "en" => "English",
        _ => culture
    };

    private static readonly Dictionary<string, Dictionary<string, string>> Strings = new()
    {
        ["tr"] = new()
        {
            ["skip"] = "İçeriğe atla",
            ["home"] = "Ana sayfa",
            ["nav.projects"] = "Projeler",
            ["nav.about"] = "Hakkımda",
            ["nav.contact"] = "İletişim",
            ["cv"] = "CV",
            ["cv.aria"] = "CV'yi indir (PDF)",
            ["social.linkedin"] = "LinkedIn profili",
            ["social.github"] = "GitHub profili",
            ["social.email"] = "E-posta gönder",
            ["theme.toggle"] = "Koyu/açık tema değiştir",
            ["lang.toggle"] = "Dili değiştir",
            ["footer"] = "El yazımı CSS ve sade JavaScript ile, framework'süz yapıldı.",

            ["hub.eyebrow"] = "// merhaba",
            ["hub.status.ok"] = "Sistemler çalışıyor",
            ["hub.status.down"] = "Sistemlerde aksama var",
            ["hub.status.offline"] = "Bağlantı kurulamadı",
            ["hub.enter"] = "Nereye gitmek istersiniz?",
            ["hub.projects.desc"] = "Çözdüğüm gerçek problemler, hikâyeleriyle",
            ["hub.about.desc"] = "Nasıl çalışırım, neye önem veririm",
            ["hub.contact.desc"] = "Bir fikriniz mi var? Yazın",

            ["projects.title"] = "Seçilmiş Projeler",
            ["projects.sub"] = "Her proje bir hikâye: ortada bir sorun vardı, bir yol seçtim, bir sonuç çıktı.",
            ["projects.more"] = "Hikâyenin tamamı",
            ["projects.tech"] = "Kullanılan teknolojiler",
            ["projects.empty"] = "Henüz yayınlanmış proje yok.",

            ["about.title"] = "Hakkımda",
            ["principles.title"] = "Nasıl Çalışırım",
            ["principles.sub"] = "Teknoloji listesi yerine üç ilke — çünkü araçlar değişir, yaklaşım kalır.",
            ["now.label"] = "Şu an",
            ["now.updated"] = "Son güncelleme",

            ["contact.title"] = "İletişim",
            ["contact.intro"] = "Bir proje, bir soru ya da sadece merhaba demek için — mesajınız bana ulaşır.",
            ["contact.name"] = "Adınız",
            ["contact.email"] = "E-posta",
            ["contact.message"] = "Mesajınız",
            ["contact.send"] = "Mesajı gönder",
            ["contact.sending"] = "Gönderiliyor…",
            ["contact.success"] = "Mesajın ulaştı, 24 saat içinde dönerim.",
            ["contact.direct"] = "Doğrudan yazmak isterseniz:",
            ["contact.noscript"] = "Form için JavaScript gerekli. Dilerseniz doğrudan e-posta gönderebilirsiniz.",

            ["detail.crumb.home"] = "ana sayfa",
            ["detail.crumb.projects"] = "projeler",
            ["detail.problem"] = "Sorun",
            ["detail.approach"] = "Yaklaşım",
            ["detail.outcome"] = "Sonuç",
            ["detail.demo"] = "Canlı demoyu gör",
            ["detail.repo"] = "Kaynak kodu",
            ["detail.back"] = "Tüm projelere dön",
            ["detail.cover.alt"] = "ekran görüntüsü",
        },
        ["en"] = new()
        {
            ["skip"] = "Skip to content",
            ["home"] = "Home",
            ["nav.projects"] = "Projects",
            ["nav.about"] = "About",
            ["nav.contact"] = "Contact",
            ["cv"] = "CV",
            ["cv.aria"] = "Download CV (PDF)",
            ["social.linkedin"] = "LinkedIn profile",
            ["social.github"] = "GitHub profile",
            ["social.email"] = "Send an email",
            ["theme.toggle"] = "Toggle dark/light theme",
            ["lang.toggle"] = "Change language",
            ["footer"] = "Built by hand with plain CSS and vanilla JavaScript — no frameworks.",

            ["hub.eyebrow"] = "// hello",
            ["hub.status.ok"] = "All systems operational",
            ["hub.status.down"] = "Systems degraded",
            ["hub.status.offline"] = "Connection failed",
            ["hub.enter"] = "Where would you like to go?",
            ["hub.projects.desc"] = "Real problems I solved, with their stories",
            ["hub.about.desc"] = "How I work, what I care about",
            ["hub.contact.desc"] = "Got an idea? Drop me a line",

            ["projects.title"] = "Selected Projects",
            ["projects.sub"] = "Every project is a story: there was a problem, I chose a path, a result came out.",
            ["projects.more"] = "Read the full story",
            ["projects.tech"] = "Technologies used",
            ["projects.empty"] = "No published projects yet.",

            ["about.title"] = "About",
            ["principles.title"] = "How I Work",
            ["principles.sub"] = "Three principles instead of a tech list — because tools change, the approach stays.",
            ["now.label"] = "Now",
            ["now.updated"] = "Last updated",

            ["contact.title"] = "Contact",
            ["contact.intro"] = "For a project, a question, or just to say hello — your message reaches me.",
            ["contact.name"] = "Your name",
            ["contact.email"] = "Email",
            ["contact.message"] = "Your message",
            ["contact.send"] = "Send message",
            ["contact.sending"] = "Sending…",
            ["contact.success"] = "Your message arrived, I'll reply within 24 hours.",
            ["contact.direct"] = "If you'd rather write directly:",
            ["contact.noscript"] = "The form needs JavaScript. You can also email me directly.",

            ["detail.crumb.home"] = "home",
            ["detail.crumb.projects"] = "projects",
            ["detail.problem"] = "Problem",
            ["detail.approach"] = "Approach",
            ["detail.outcome"] = "Outcome",
            ["detail.demo"] = "View live demo",
            ["detail.repo"] = "Source code",
            ["detail.back"] = "Back to all projects",
            ["detail.cover.alt"] = "screenshot",
        }
    };
}
