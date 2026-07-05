using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;

namespace Portfolio.Web.Features.Health;

/// <summary>Hero'daki "● Sistemler çalışıyor" rozetinin gerçek kaynağı.</summary>
public static class HealthEndpoint
{
    public static IEndpointRouteBuilder MapHealth(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/health", async (AppDbContext db, CancellationToken ct) =>
        {
            var dbOk = await db.Database.CanConnectAsync(ct);
            return dbOk
                ? Results.Ok(new { status = "ok" })
                : Results.Json(new { status = "degraded" }, statusCode: StatusCodes.Status503ServiceUnavailable);
        });

        return app;
    }
}
