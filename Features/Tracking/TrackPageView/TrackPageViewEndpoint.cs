namespace Portfolio.Web.Features.Tracking.TrackPageView;

public static class TrackPageViewEndpoint
{
    public static IEndpointRouteBuilder MapTrackPageView(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/track", async (
            TrackPageViewRequest request,
            TrackPageViewHandler handler,
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var clientIp = httpContext.Connection.RemoteIpAddress?.ToString();
            await handler.HandleAsync(request, clientIp, ct);
            return Results.Accepted();
        });

        return app;
    }
}
