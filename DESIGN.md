# Tasarım Planı — "Gece Yarısı Şeması" (Blueprint)

## Konsept

Site, **iyi düzenlenmiş bir teknik çizim** hissi verir: hassas, sakin, mühendisçe —
ama teknik çizimlerin aksine her notu insan dilinde. Teknik olmayan ziyaretçi temiz ve
sıcak bir sayfa görür; yazılımcı, ince blueprint grid'ini, monospace anotasyonları ve
gerçek health-check'e bağlı durum rozetini fark eder.

Boldness bütçesinin tamamı açılıştaki **"sistem uyanıyor" sekansına** harcanır.
Sayfanın geri kalanı disiplinli ve dingin kalır; tek cesur renk, tek imza an.

## Renk Paleti

| İsim | Rol | Açık mod | Koyu mod |
|---|---|---|---|
| **Kâğıt** | zemin | `#F4F6F9` | `#0D141F` |
| **Zemin Kartı** | kart/yüzey | `#FFFFFF` | `#141E2C` |
| **Mürekkep** | ana metin | `#17212F` | `#E8EDF4` |
| **Kurşun** | ikincil metin | `#5A6A7E` | `#94A4B8` |
| **Çini Mavisi** | grid/çizgi/şema dokusu | `#39628F` (düşük opaklıkla) | `#3E6795` (düşük opaklıkla) |
| **İşaret Turuncusu** | tek cesur aksan | `#D4530E` | `#FF7A3D` |
| **Onay Yeşili** | canlı durum noktası | `#1F8A54` | `#3ECF8E` |

**Aksan gerekçesi:** İşaret turuncusu, teknik çizimlerde revizyon/dikkat işaretlerinin
rengidir — soğuk mavi-gri şema zemini üzerinde tek sıcak vurgu olarak hem mühendislik
metaforunu sürdürür hem de yasaklı kombinasyonların (krem+terracotta, siyah+asit yeşili,
mor/pembe gradient) hiçbirine benzemez: zemin soğuk kâğıt beyazı/gece laciverti, aksan
markör turuncusu.

## Tipografi (üçlü sistem, tamamı latin-ext / tam Türkçe destekli, self-host woff2)

| Rol | Aile | Kullanım |
|---|---|---|
| **Display** | Sora (600/700) | Bölüm başlıkları, büyük isim. Ölçülü: yalnızca başlık. |
| **Gövde** | Inter (400/600) | Paragraflar, form, kartlar. |
| **Mono** | JetBrains Mono (400/500) | Boot sekansı, tech rozetleri, bölüm numaraları, durum rozeti. |

`font-display: swap`, dosyalar `wwwroot/fonts/` altında self-host (üçüncü taraf istek yok).

## Doku

- **Blueprint grid:** Zeminde 48px aralıklı, %4-6 opaklıkta çini mavisi çizgiler
  (CSS gradient ile, resim yok). Kartların içinde değil, yalnızca sayfa zemininde.
- **Hairline ayraçlar:** Bölümler 1px çizgi + sol üstte monospace bölüm etiketi
  (`01 · Projeler` gibi) ile ayrılır — şema paftası anotasyonu gibi.
- **Grain:** Yok. (Grid + hairline yeter; doku enflasyonundan kaçın.)

## Layout

- Tek sütun akış, `max-width: 68rem`, bol beyaz alan; mobile-first.
- Hero tam ekran yüksekliğe yakın; isim display ile büyük, altında DB'den gelen
  tek cümle pozisyonlama; sağ üstte canlı "● Sistemler çalışıyor" rozeti.
- Projeler: dikey kart listesi (grid değil) — her kart Sorun → Yaklaşım → Sonuç
  anlatısını üç kısa blok halinde taşır; tech rozetleri altta, hover/focus'ta
  insan dili tooltip.
- "Nasıl çalışırım": 3 ilke kartı yatay (mobilde dikey).
- "Şu an": tek satırlık canlı widget, nabız atan nokta ile.
- Hakkımda + iletişim: iki sütun (mobilde tek), form + direkt e-posta.

## İmza Öğe

**Boot sekansı:** Sayfa ilk açılışta koyu şema zemininde monospace, satır satır
Türkçe durum mesajları (`Merhaba ✓`, `Projeler yükleniyor ✓`, `Kahve seviyesi:
yeterli ✓`) akar; ≤2.5 sn, tıklama/tuş ile atlanabilir, `prefers-reduced-motion`
ve tekrar ziyarette (sessionStorage) hiç oynatılmaz. Bittiğinde içerik yumuşak
fade+rise ile belirir ve hero'daki durum rozeti gerçek `/api/health` çağrısıyla
"● Sistemler çalışıyor"a döner — jargon değil, herkesin anlayacağı bir metafor.

## Hareket

- Scroll'da bölümler `IntersectionObserver` ile bir kez, 400ms fade+8px rise.
- Hover mikro-etkileşimler: kart hairline'ının aksana dönmesi, rozet tooltip'i.
- Tüm animasyonlar `prefers-reduced-motion: reduce` altında kapalı.

## Klişe Testi (yasak listesi kontrolü)

- [x] Mor/pembe gradient hero — **yok** (düz soğuk zemin + grid dokusu)
- [x] Krem zemin + terracotta — **yok** (zemin soğuk mavi-beyaz `#F4F6F9`, krem değil)
- [x] Siyah zemin + asit yeşili — **yok** (koyu mod gece laciverti + turuncu; yeşil yalnız minik durum noktası)
- [x] "Hi, I'm X 👋" — **yok** (hero doğrudan isim + pozisyonlama cümlesi, emoji selamlama yok)
- [x] Skill progress bar — **yok** (yerine 3 çalışma ilkesi kartı)
