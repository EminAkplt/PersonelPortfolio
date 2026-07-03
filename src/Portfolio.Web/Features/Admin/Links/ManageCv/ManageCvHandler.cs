using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Common.Caching;
using Portfolio.Web.Data;
using Portfolio.Web.Domain;
using Portfolio.Web.Features.Content.GetSiteContent;

namespace Portfolio.Web.Features.Admin.Links.ManageCv;

/// <summary>CV (PDF) yükleme/kaldırma; dosya wwwroot/files altında tutulur, yol SiteContent'e yazılır.</summary>
public sealed class ManageCvHandler(AppDbContext db, IAppCache cache, IWebHostEnvironment env)
{
    private const long MaxBytes = 10 * 1024 * 1024; // 10 MB
    private const string RelativeDir = "files";

    public async Task<Result> UploadAsync(IFormFile? file, CancellationToken ct = default)
    {
        if (file is null || file.Length == 0)
            return Result.Fail(Error.Validation("cv_empty", "Bir PDF dosyası seçmelisiniz."));

        if (file.Length > MaxBytes)
            return Result.Fail(Error.Validation("cv_too_large", "CV dosyası 10 MB'ı aşamaz."));

        var isPdf = file.ContentType == "application/pdf"
            || Path.GetExtension(file.FileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase);
        if (!isPdf)
            return Result.Fail(Error.Validation("cv_not_pdf", "Yalnızca PDF dosyası yükleyebilirsiniz."));

        var webRoot = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
        var dir = Path.Combine(webRoot, RelativeDir);
        Directory.CreateDirectory(dir);

        // Önceki CV'yi bul (silmek için)
        var previous = await db.SiteContents.FirstOrDefaultAsync(c => c.Key == SiteContentKeys.CvFile, ct);

        var fileName = $"cv-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.pdf";
        var fullPath = Path.Combine(dir, fileName);

        await using (var stream = File.Create(fullPath))
        {
            await file.CopyToAsync(stream, ct);
        }

        var relativePath = $"/{RelativeDir}/{fileName}";
        await UpsertAsync(previous, relativePath, ct);

        TryDeletePrevious(webRoot, previous?.Value);
        cache.Remove(GetSiteContentHandler.CacheKey);
        return Result.Success();
    }

    public async Task<Result> RemoveAsync(CancellationToken ct = default)
    {
        var previous = await db.SiteContents.FirstOrDefaultAsync(c => c.Key == SiteContentKeys.CvFile, ct);
        if (previous is null || string.IsNullOrEmpty(previous.Value))
            return Result.Success();

        var webRoot = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
        TryDeletePrevious(webRoot, previous.Value);

        db.SiteContents.Remove(previous);
        await db.SaveChangesAsync(ct);

        cache.Remove(GetSiteContentHandler.CacheKey);
        return Result.Success();
    }

    private async Task UpsertAsync(SiteContent? existing, string value, CancellationToken ct)
    {
        if (existing is null)
            db.SiteContents.Add(new SiteContent { Key = SiteContentKeys.CvFile, Value = value, UpdatedAt = DateTimeOffset.UtcNow });
        else
        {
            existing.Value = value;
            existing.UpdatedAt = DateTimeOffset.UtcNow;
        }

        await db.SaveChangesAsync(ct);
    }

    private static void TryDeletePrevious(string webRoot, string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath) || !relativePath.StartsWith($"/{RelativeDir}/"))
            return;

        try
        {
            var full = Path.Combine(webRoot, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(full))
                File.Delete(full);
        }
        catch
        {
            // Eski dosya silinemezse sorun değil; yeni yol zaten kaydedildi.
        }
    }
}
