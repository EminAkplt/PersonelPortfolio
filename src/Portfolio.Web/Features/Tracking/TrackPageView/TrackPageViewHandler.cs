using Portfolio.Web.Common.Security;
using Portfolio.Web.Data;
using Portfolio.Web.Domain;

namespace Portfolio.Web.Features.Tracking.TrackPageView;

/// <summary>
/// Fire-and-forget sayfa görüntüleme kaydı. Analitik hiçbir zaman
/// kullanıcı deneyimini bozmamalı; bu yüzden tüm hatalar yutulur ve loglanır.
/// </summary>
public sealed class TrackPageViewHandler(
    AppDbContext db,
    IpHasher ipHasher,
    ILogger<TrackPageViewHandler> logger)
{
    public async Task HandleAsync(TrackPageViewRequest request, string? clientIp, CancellationToken ct = default)
    {
        try
        {
            var path = request.Path?.Trim();
            if (string.IsNullOrEmpty(path) || !path.StartsWith('/') || path.Length > 300)
                return;

            db.PageViews.Add(new PageView
            {
                Path = path,
                ClientIpHash = ipHasher.Hash(clientIp),
                VisitedAt = DateTimeOffset.UtcNow
            });

            await db.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Sayfa görüntüleme kaydı başarısız oldu; istek etkilenmedi.");
        }
    }
}
