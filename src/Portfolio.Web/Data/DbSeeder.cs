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

        var seedProjects = new[]
        {
                new Project
                {
                    Slug = "stok-takip-sistemi",
                    Title = "Depo Stok Takip Sistemi",
                    Summary = "Bir yapı market zincirinin elle tuttuğu stok defterini, hatasız çalışan bir web uygulamasına dönüştürdüm.",
                    Problem = "Üç şubeli bir yapı market, stoklarını Excel ve kâğıt defterlerle takip ediyordu. Şubeler arası stok bilgisi telefonla soruluyor, ayda ortalama 40 saat sayım farkı düzeltmeye harcanıyordu.",
                    Approach = "Önce iki gün sahada çalışıp mevcut süreci izledim. Barkod okuyucuyla çalışan, çevrimdışı da kayıt tutabilen basit bir web uygulaması tasarladım. Karmaşık ERP modülleri yerine, ekibin gerçekten kullandığı üç işlemi (giriş, çıkış, sayım) merkeze aldım.",
                    Outcome = "Sayım farkı düzeltme süresi ayda 40 saatten 4 saate indi. Sistem 8 aydır kesintisiz çalışıyor; ekip hiçbir eğitim dokümanına ihtiyaç duymadan kullanmaya başladı.",
                    TitleEn = "Warehouse Stock Tracking System",
                    SummaryEn = "I turned a hardware store chain's hand-kept stock ledger into a web app that just works.",
                    ProblemEn = "A hardware store with three branches tracked its stock with Excel and paper ledgers. Stock levels between branches were asked over the phone, and about 40 hours a month were spent fixing counting discrepancies.",
                    ApproachEn = "I spent two days on site watching the actual process first. I designed a simple web app that works with a barcode scanner and keeps records offline too. Instead of complex ERP modules, I centered the three operations the team actually uses (in, out, count).",
                    OutcomeEn = "Time spent correcting counting errors dropped from 40 hours a month to 4. The system has run without interruption for 8 months; the team started using it without needing any training document.",
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
                    TitleEn = "Clinic Appointment Assistant",
                    SummaryEn = "I moved a dental clinic's phone-lost appointments to an online system that takes 30 seconds per patient.",
                    ProblemEn = "The clinic's receptionist took more than 60 calls a day, and every patient left on hold was a potential loss. Appointment clashes happened a few times a week and hurt patient satisfaction.",
                    ApproachEn = "I built a single-page booking flow patients can use without downloading an app or signing up. I designed a calendar model that makes clashes impossible and automated SMS reminders.",
                    OutcomeEn = "70% of appointments are now booked without a phone call. No-show rate dropped 35% thanks to SMS reminders. The receptionist can focus on greeting patients instead of phone traffic.",
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
                    TitleEn = "Field Team Reporting App",
                    SummaryEn = "I turned an electrical company's paper work orders into a digital form filled on-site from a phone and delivered to HQ instantly.",
                    ProblemEn = "Field technicians wrote work orders on paper, and forms reached HQ once a week. Billing was delayed, and dozens of forms a month were returned because of unreadable handwriting.",
                    ApproachEn = "I designed a mobile web form with large touch targets that technicians can use even with gloves and attach photos. For basements with no internet, I added offline capture and automatic sync.",
                    OutcomeEn = "Billing delay dropped from 9 days on average to 1. Returned forms fell to zero. The company was able to add two new field teams to the system on its own.",
                    TechStack = ["ASP.NET Core", "PostgreSQL", "PWA", "IndexedDB"],
                    DisplayOrder = 3,
                    IsPublished = true,
                    CreatedAt = now,
                    UpdatedAt = now
                }
        };

        if (!await db.Projects.AnyAsync())
        {
            db.Projects.AddRange(seedProjects);
        }
        else
        {
            // Daha önce (EN alanları eklenmeden) seed edilmiş projelere İngilizce çevirileri geriye doldur.
            var bySlug = seedProjects.ToDictionary(p => p.Slug);
            var toBackfill = await db.Projects.Where(p => p.TitleEn == null).ToListAsync();
            foreach (var cur in toBackfill)
            {
                if (bySlug.TryGetValue(cur.Slug, out var sp) && sp.TitleEn is not null)
                {
                    cur.TitleEn = sp.TitleEn;
                    cur.SummaryEn = sp.SummaryEn;
                    cur.ProblemEn = sp.ProblemEn;
                    cur.ApproachEn = sp.ApproachEn;
                    cur.OutcomeEn = sp.OutcomeEn;
                    cur.UpdatedAt = now;
                }
            }
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
            // Dilden bağımsız
            [SiteContentKeys.HeroName] = "Emin",
            [SiteContentKeys.ContactEmail] = "mirainternational34@gmail.com",
            [SiteContentKeys.SocialLinkedin] = "https://www.linkedin.com/",
            [SiteContentKeys.SocialGithub] = "https://github.com/EminAkplt",

            // TR (temel)
            [SiteContentKeys.HeroTagline] = "Karmaşık problemleri, insanların severek kullandığı basit yazılımlara dönüştürüyorum.",
            [SiteContentKeys.AboutText] = "Merhaba, ben Emin. Yazılımı, teknolojiye uzak insanların hayatını kolaylaştırdığında anlamlı buluyorum. Bir sistemin arkasında ne kadar incelikli mühendislik olursa olsun, kullanan kişi bunu fark etmiyorsa doğru yoldayım demektir. Web uygulamaları geliştiriyorum; sorunu dinleyerek başlıyor, basit tutarak ilerliyor ve kırılmadığından emin olarak teslim ediyorum.",
            [SiteContentKeys.Principle1Title] = "Önce sorunu anlarım",
            [SiteContentKeys.Principle1Text] = "Kod yazmadan önce işin sahibiyle konuşur, süreci yerinde izlerim. Yanlış soruya verilen mükemmel cevap, kimseye fayda sağlamaz.",
            [SiteContentKeys.Principle2Title] = "Basit tutarım",
            [SiteContentKeys.Principle2Text] = "En iyi çözüm, bakımı kolay olandır. Gösterişli teknolojiler yerine, beş yıl sonra da anlaşılır kalacak sade yapılar kurarım.",
            [SiteContentKeys.Principle3Title] = "Kırılmadığından emin olurum",
            [SiteContentKeys.Principle3Text] = "Teslim ettiğim her sistemin testleri, yedekleri ve izleme düzeni vardır. Gece uyandıran yazılım, bitmemiş yazılımdır.",

            // EN (".en" varyantları)
            [$"{SiteContentKeys.HeroTagline}.en"] = "I turn complex problems into simple software people love to use.",
            [$"{SiteContentKeys.AboutText}.en"] = "Hi, I'm Emin. I find software meaningful when it makes life easier for people far from technology. No matter how refined the engineering behind a system is, if the person using it doesn't notice, I'm on the right track. I build web applications; I start by listening to the problem, keep it simple, and deliver making sure it won't break.",
            [$"{SiteContentKeys.Principle1Title}.en"] = "I understand the problem first",
            [$"{SiteContentKeys.Principle1Text}.en"] = "Before writing code I talk to the owner and watch the process on site. A perfect answer to the wrong question helps no one.",
            [$"{SiteContentKeys.Principle2Title}.en"] = "I keep it simple",
            [$"{SiteContentKeys.Principle2Text}.en"] = "The best solution is the one that's easy to maintain. Instead of flashy tech, I build plain structures that will still make sense five years from now.",
            [$"{SiteContentKeys.Principle3Title}.en"] = "I make sure it won't break",
            [$"{SiteContentKeys.Principle3Text}.en"] = "Every system I deliver has tests, backups and monitoring. Software that wakes you up at night is unfinished software."
        };

        var existingKeys = (await db.SiteContents.Select(c => c.Key).ToListAsync()).ToHashSet();
        foreach (var (key, value) in defaults)
        {
            if (!existingKeys.Contains(key))
                db.SiteContents.Add(new SiteContent { Key = key, Value = value, UpdatedAt = now });
        }

        await db.SaveChangesAsync();
    }
}
