# Kişisel Portfolio — Emin

Dinamik, iki kitleye birden konuşan bir yazılım mühendisi portfolio sitesi:
teknik olmayan ziyaretçi her şeyi anlar ("çevirili teknik dil" prensibi),
yazılımcı ziyaretçi detaylardaki işçiliği fark eder.

Tasarım kararları için: [DESIGN.md](DESIGN.md)

## Öne çıkanlar

- **İmza açılış:** ≤2.5 sn "sistem uyanıyor" sekansı — Türkçe, atlanabilir,
  `prefers-reduced-motion` ve tekrar ziyarette hiç oynatılmaz
- **Canlı durum rozeti:** Hero'daki "● Sistemler çalışıyor" gerçekten
  `/api/health`'e bağlanır
- **Hikâye odaklı projeler:** Sorun → Yaklaşım → Sonuç; tech rozetlerinde
  insan dilinde tooltip'ler
- **Tamamı dinamik:** projeler, metinler, "şu an" durumu ve mesajlar
  PostgreSQL'de; `/admin` panelinden yönetilir
- Framework'süz el yazımı CSS (custom properties, container queries) +
  vanilla JS (ES modülleri); koyu/açık tema

## Teknoloji

| Katman | Seçim |
|---|---|
| Backend | .NET 10, ASP.NET Core (Razor Pages + Minimal API) |
| Mimari | Vertical slice — her feature kendi klasöründe plain handler (MediatR yok) |
| Hata yönetimi | `Result<T>` pattern (exception'la akış kontrolü yok) |
| Veritabanı | PostgreSQL 16+ (EF Core, code-first migrations) |
| Cache | In-memory (`IAppCache` soyutlaması Redis'e geçişe hazır) |
| Doğrulama | FluentValidation |
| Frontend | El yazımı CSS + vanilla JS — framework/Tailwind/Bootstrap yok |

## Lokal kurulum

Gereksinimler: [.NET 10 SDK](https://dotnet.microsoft.com/download), lokal PostgreSQL 16+.

1. **Veritabanı ve kullanıcıyı oluşturun** (bir kere):

   ```sql
   -- psql ile postgres süper kullanıcısı olarak:
   CREATE ROLE portfolio LOGIN PASSWORD 'portfolio';
   CREATE DATABASE portfolio OWNER portfolio;
   ```

2. **Bağlantı cümlesini kontrol edin** — varsayılan
   [appsettings.json](src/Portfolio.Web/appsettings.json) şu şekildedir;
   kendi kurulumunuza göre düzenleyin veya ortam değişkeniyle ezin:

   ```
   Host=localhost;Port=5432;Database=portfolio;Username=portfolio;Password=portfolio
   ```

   Ortam değişkeniyle: `ConnectionStrings__Default=...`

3. **Çalıştırın:**

   ```bash
   dotnet run --project src/Portfolio.Web
   ```

   İlk açılışta migration'lar otomatik uygulanır ve örnek içerik seed edilir.

## Admin paneli

- Adres: `/admin/giris`
- **Geliştirme ortamında** varsayılan giriş: `admin` / `admin123`
  (yalnızca `Admin:PasswordHash` boşken ve ortam Development iken çalışır)
- **Üretimde** hash zorunludur — hash'i üretin ve ortam değişkeni olarak verin:

  ```bash
  dotnet run --project src/Portfolio.Web -- hash-password "güçlü-şifreniz"
  # çıktıyı kopyalayın:
  # Admin__PasswordHash=100000.xxxx.yyyy
  # Admin__Username=admin        (isteğe bağlı, varsayılan: admin)
  ```

Panelden yapılabilenler: proje CRUD + yayınla/gizle + sıralama, gelen
mesajlar (okundu/sil), hero ve hakkımda metinleri, "şu an" durumu,
son 30 gün görüntülenme grafiği.

## Deploy (Docker'sız, klasik yayınlama)

```bash
dotnet publish src/Portfolio.Web -c Release -o publish
```

`publish/` klasörünü sunucuya kopyalayıp `Portfolio.Web.dll`'i çalıştırın.
Örnek systemd servisi:

```ini
[Unit]
Description=Portfolio
After=network.target postgresql.service

[Service]
WorkingDirectory=/var/www/portfolio
ExecStart=/usr/bin/dotnet /var/www/portfolio/Portfolio.Web.dll
Restart=always
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://127.0.0.1:5000
Environment=ConnectionStrings__Default=Host=localhost;Port=5432;Database=portfolio;Username=portfolio;Password=***
Environment=Admin__PasswordHash=100000.xxxx.yyyy
Environment=Security__IpHashSalt=rastgele-uzun-bir-deger
# Nginx/Caddy arkasındaysanız gerçek istemci IP'si için:
Environment=ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

[Install]
WantedBy=multi-user.target
```

Önünde Nginx/Caddy gibi TLS sonlandıran bir reverse proxy önerilir.
`ASPNETCORE_FORWARDEDHEADERS_ENABLED=true` olmadan rate limit ve IP hash
proxy'nin IP'sini görür.

## Proje yapısı

```
src/Portfolio.Web/
├── Common/            # Result pattern, cache soyutlaması, güvenlik yardımcıları
├── Data/              # DbContext, migrations, seed
├── Domain/            # Entity'ler
├── Features/          # Vertical slice'lar (endpoint + handler + DTO + validator)
│   ├── Projects/ Contact/ Now/ Tracking/ Health/ Content/ Seo/
│   └── Admin/         # Admin use-case'leri
├── Pages/             # Razor Pages (public + /admin)
└── wwwroot/           # El yazımı CSS, ES modülleri, self-host fontlar
```

## Notlar

- Kişisel veri: ham IP saklanmaz; iletişim ve analitik kayıtlarında
  yalnızca salt'lı SHA256 hash tutulur (KVKK dostu).
- İletişim formu: IP başına saatte 3 mesaj rate limit + honeypot alanı.
- Güvenlik: nonce'lu CSP, X-Frame-Options, nosniff, anti-forgery (admin),
  PBKDF2 şifre hash'i.
