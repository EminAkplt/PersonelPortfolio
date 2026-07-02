using Portfolio.Web.Common.Http;

namespace Portfolio.Web.Features.Projects.GetProjects;

public static class GetProjectsEndpoint
{
    public static IEndpointRouteBuilder MapGetProjects(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/projects", async (GetProjectsHandler handler, CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(ct);
            return result.ToHttpResult();
        });

        return app;
    }
}
