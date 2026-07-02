using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Domain;

namespace Portfolio.Web.Data;

/// <summary>Uygulama açılışında migration'ları uygular ve boş tabloları başlangıç içeriğiyle doldurur.</summary>
public static class DbSeeder
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.MigrateAsync();

        var now = DateTimeOffset.UtcNow;

        if (!await db.Projects.AnyAsync())
        {
            db.Projects.AddRange(
                new Project
                {
                    Slug = "stok-takip-sistemi",
                    Title = "Depo Stok Takip Sistemi",
                    Summary = "Bir yapı market zincirinin elle tuttuğu stok defterini, hatasız çalışan bir web uygulamasına dönüştürdüm.",
                    Problem = "Üç şubeli bir yapı market, stoklarını Excel ve kâğıt defterlerle takip ediyordu. Şubeler arası stok bilgisi telefonla soruluyor, ayda ortalama 40 saat sayım farkı düzeltmeye harcanıyordu.",
                    Approach = "Önce iki gün sahada çalışıp mevcut süreci izledim. Barkod okuyucuyla çalışan, çevrimdışı da kayıt tutabilen basit bir web uygulaması tasarladım. Karmaşık ERP modülleri yerine, ekibin gerçekten kullandığı üç işlemi (giriş, çıkış, sayım) merkeze aldım.",
                    Outcome = "Sayım farkı düzeltme süresi ayda 40 saatten 4 saate indi. Sistem 8 aydır kesintisiz çalışıyor; ekip hiçbir eğitim dokümanına ihtiyaç duymadan kullanmaya başladı.",
                    TechStack = ["ASP.NET Core", "PostgreSQL", "Vanilla JS"],
                    DisplayOrder = 1,
                    IsPublished = true,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Project
                {
                    Slug = "randevu-asistani",
                    Title = "Klinik Randevu Asistanı",
                    Summary = "Bir diş kliniğinin telefonda kaybettiği randevuları, hasta başına 30 saniyede tamamlanan bir online sisteme taşıdım.",
                    Problem = "Klinik sekreteri günde 60'tan fazla telefon alıyor, hatta kalınan her hasta potansiyel bir kayıptı. Randevu çakışmaları haftada birkaç kez yaşanıyor ve hasta memnuniyetini düşürüyordu.",
                    Approach = "Hastaların uygulama indirmeden, üye olmadan kullanabileceği tek sayfalık bir randevu akışı kurdum. Çakışmaları imkânsız kılan bir takvim modeli tasarladım; SMS hatırlatmalarını otomatikleştirdim.",
                    Outcome = "Randevuların %70'i artık telefonsuz alınıyor. Randevuya gelmeme oranı SMS hatırlatmalarıyla %35 azaldı. Sekreter, telefon trafiği yerine hasta karşılamaya odaklanabiliyor.",
                    TechStack = ["ASP.NET Core", "PostgreSQL", "Redis", "Twilio"],
                    DisplayOrder = 2,
                    IsPublished = true,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Project
                {
                    Slug = "saha-rapor-uygulamasi",
                    Title = "Saha Ekibi Rapor Uygulaması",
                    Summary = "Elektrik tesisat firmasının kâğıt iş emirlerini, sahada telefondan doldurulan ve anında merkeze düşen dijital forma çevirdim.",
                    Problem = "Saha teknisyenleri iş emirlerini kâğıda yazıyor, formlar haftada bir merkeze ulaşıyordu. Faturalama gecikiyor, okunmayan el yazıları yüzünden ayda onlarca form geri dönüyordu.",
                    Approach = "Teknisyenlerin eldivenle bile kullanabileceği, büyük dokunmatik alanlı, fotoğraf ekleyebilen bir mobil web formu tasarladım. İnternet olmayan bodrum katlarında da çalışması için çevrimdışı kayıt ve otomatik senkronizasyon ekledim.",
                    Outcome = "Faturalama gecikmesi ortalama 9 günden 1 güne indi. Geri dönen form sayısı sıfırlandı. Firma iki yeni saha ekibini sisteme kendi başına ekleyebildi.",
                    TechStack = ["ASP.NET Core", "PostgreSQL", "PWA", "IndexedDB"],
                    DisplayOrder = 3,
                    IsPublished = true,
                    CreatedAt = now,
                    UpdatedAt = now
                });
        }

        if (!await db.NowStatuses.AnyAsync())
        {
            db.NowStatuses.Add(new NowStatus
            {
                StatusText = "Arapça destekli bir yapay zekâ uygulaması geliştiriyorum",
                Mood = Mood.Building,
                UpdatedAt = now
            });
        }

        var defaults = new Dictionary<string, string>
        {
            [SiteContentKeys.HeroName] = "Emin",
            [SiteContentKeys.HeroTagline] = "Karmaşık problemleri, insanların severek kullandığı basit yazılımlara dönüştürüyorum.",
            [SiteContentKeys.AboutText] = "Merhaba, ben Emin. Yazılımı, teknolojiye uzak insanların hayatını kolaylaştırdığında anlamlı buluyorum. Bir sistemin arkasında ne kadar incelikli mühendislik olursa olsun, kullanan kişi bunu fark etmiyorsa doğru yoldayım demektir. Web uygulamaları geliştiriyorum; sorunu dinleyerek başlıyor, basit tutarak ilerliyor ve kırılmadığından emin olarak teslim ediyorum.",
            [SiteContentKeys.Principle1Title] = "Önce sorunu anlarım",
            [SiteContentKeys.Principle1Text] = "Kod yazmadan önce işin sahibiyle konuşur, süreci yerinde izlerim. Yanlış soruya verilen mükemmel cevap, kimseye fayda sağlamaz.",
            [SiteContentKeys.Principle2Title] = "Basit tutarım",
            [SiteContentKeys.Principle2Text] = "En iyi çözüm, bakımı kolay olandır. Gösterişli teknolojiler yerine, beş yıl sonra da anlaşılır kalacak sade yapılar kurarım.",
            [SiteContentKeys.Principle3Title] = "Kırılmadığından emin olurum",
            [SiteContentKeys.Principle3Text] = "Teslim ettiğim her sistemin testleri, yedekleri ve izleme düzeni vardır. Gece uyandıran yazılım, bitmemiş yazılımdır.",
            [SiteContentKeys.ContactEmail] = "mirainternational34@gmail.com"
        };

        var existingKeys = await db.SiteContents.Select(c => c.Key).ToListAsync();
        foreach (var (key, value) in defaults)
        {
            if (!existingKeys.Contains(key))
                db.SiteContents.Add(new SiteContent { Key = key, Value = value, UpdatedAt = now });
        }

        await db.SaveChangesAsync();
    }
}
