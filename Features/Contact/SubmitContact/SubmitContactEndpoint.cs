using Portfolio.Web.Common.Http;

namespace Portfolio.Web.Features.Contact.SubmitContact;

public static class SubmitContactEndpoint
{
    public const string RateLimitPolicy = "contact";

    public static IEndpointRouteBuilder MapSubmitContact(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/contact", async (
            SubmitContactRequest request,
            SubmitContactHandler handler,
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var clientIp = httpContext.Connection.RemoteIpAddress?.ToString();
            var result = await handler.HandleAsync(request, clientIp, ct);
            return result.IsSuccess
                ? Results.Ok(new { message = "Mesajın ulaştı, 24 saat içinde dönerim." })
                : result.Error!.ToProblem();
        })
        .RequireRateLimiting(RateLimitPolicy);

        return app;
    }
}
