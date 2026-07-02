using Portfolio.Web.Common.Http;

namespace Portfolio.Web.Features.Now.GetNow;

public static class GetNowEndpoint
{
    public static IEndpointRouteBuilder MapGetNow(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/now", async (GetNowHandler handler, CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(ct);
            return result.ToHttpResult();
        });

        return app;
    }
}
