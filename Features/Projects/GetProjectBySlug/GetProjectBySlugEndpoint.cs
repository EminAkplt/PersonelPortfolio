using Portfolio.Web.Common.Http;

namespace Portfolio.Web.Features.Projects.GetProjectBySlug;

public static class GetProjectBySlugEndpoint
{
    public static IEndpointRouteBuilder MapGetProjectBySlug(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/projects/{slug}", async (string slug, GetProjectBySlugHandler handler, CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(slug, ct);
            return result.ToHttpResult();
        });

        return app;
    }
}
