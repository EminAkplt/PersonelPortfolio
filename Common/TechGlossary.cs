namespace Portfolio.Web.Common;

/// <summary>
/// "Çevirili teknik dil" sözlüğü: her teknoloji etiketinin yanında
/// teknik olmayan ziyaretçinin anlayacağı bir cümle gösterilir.
/// </summary>
public static class TechGlossary
{
    private static readonly Dictionary<string, string> Descriptions = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ASP.NET Core"] = "Sitenin beyni — istekleri karşılayan sunucu tarafı",
        [".NET"] = "Uygulamanın üzerine kurulduğu Microsoft platformu",
        ["C#"] = "Uygulamanın yazıldığı programlama dili",
        ["PostgreSQL"] = "Verilerin güvenle saklandığı yer",
        ["Redis"] = "Sık kullanılan bilgileri hızlı erişim için tutan bellek",
        ["Vanilla JS"] = "Tarayıcıda çalışan sade, eklentisiz kod",
        ["JavaScript"] = "Sayfayı tarayıcıda canlandıran dil",
        ["TypeScript"] = "Hataları daha yazarken yakalayan JavaScript",
        ["PWA"] = "Uygulama gibi kurulabilen, çevrimdışı da çalışan web sitesi",
        ["IndexedDB"] = "İnternet yokken verileri tarayıcıda saklayan depo",
        ["Twilio"] = "SMS ve aramaları otomatikleştiren servis",
        ["Docker"] = "Uygulamayı her sunucuda aynı çalıştıran paketleme",
        ["EF Core"] = "Veritabanıyla konuşmayı kolaylaştıran katman",
        ["SQL Server"] = "Verilerin güvenle saklandığı Microsoft veritabanı",
        ["Azure"] = "Uygulamanın yaşadığı Microsoft bulutu",
        ["React"] = "Arayüzü parçalardan kuran popüler kütüphane",
        ["SignalR"] = "Sayfaya anlık bildirim taşıyan bağlantı",
        ["RabbitMQ"] = "Sistemler arası mesajları sıraya koyan postacı",
        ["gRPC"] = "Servislerin birbiriyle hızlı konuşma yolu"
    };

    public static string Describe(string tech) =>
        Descriptions.GetValueOrDefault(tech, "Projede kullanılan araçlardan biri");
}
