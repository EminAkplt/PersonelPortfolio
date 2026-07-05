using System.Security.Cryptography;

namespace Portfolio.Web.Common.Http;

/// <summary>
/// Güvenlik header'ları: CSP (istek başına nonce ile), clickjacking ve
/// MIME sniffing koruması, sıkı referrer ve izin politikaları.
/// </summary>
public static class SecurityHeaders
{
    public const string NonceKey = "CSPNonce";

    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app) =>
        app.Use(async (context, next) =>
        {
            var nonce = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            context.Items[NonceKey] = nonce;

            context.Response.OnStarting(() =>
            {
                var headers = context.Response.Headers;

                headers.ContentSecurityPolicy =
                    "default-src 'self'; " +
                    $"script-src 'self' 'nonce-{nonce}'; " +
                    "style-src 'self'; " +
                    "img-src 'self' data: https:; " +
                    "font-src 'self'; " +
                    "connect-src 'self'; " +
                    "frame-ancestors 'none'; " +
                    "base-uri 'self'; " +
                    "form-action 'self'";

                headers.XFrameOptions = "DENY";
                headers.XContentTypeOptions = "nosniff";
                headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
                headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";

                return Task.CompletedTask;
            });

            await next();
        });

    public static string GetNonce(this HttpContext context) =>
        context.Items[NonceKey] as string ?? string.Empty;
}
