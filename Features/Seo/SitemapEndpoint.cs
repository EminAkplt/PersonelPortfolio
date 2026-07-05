using System.Text;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;

namespace Portfolio.Web.Features.Seo;

public static class SitemapEndpoint
{
    public static IEndpointRouteBuilder MapSitemap(this IEndpointRouteBuilder app)
    {
        app.MapGet("/sitemap.xml", async (AppDbContext db, HttpContext http, CancellationToken ct) =>
        {
            var baseUrl = $"{http.Request.Scheme}://{http.Request.Host}";

            var projects = await db.Projects
                .AsNoTracking()
                .Where(p => p.IsPublished)
                .Select(p => new { p.Slug, p.UpdatedAt })
                .ToListAsync(ct);

            var xml = new StringBuilder();
            xml.Append("""<?xml version="1.0" encoding="UTF-8"?>""");
            xml.Append("""<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">""");
            xml.Append($"<url><loc>{baseUrl}/</loc></url>");

            foreach (var p in projects)
                xml.Append($"<url><loc>{baseUrl}/proje/{p.Slug}</loc><lastmod>{p.UpdatedAt:yyyy-MM-dd}</lastmod></url>");

            xml.Append("</urlset>");

            return Results.Content(xml.ToString(), "application/xml", Encoding.UTF8);
        });

        app.MapGet("/robots.txt", (HttpContext http) =>
        {
            var baseUrl = $"{http.Request.Scheme}://{http.Request.Host}";
            var content = $"""
                User-agent: *
                Allow: /
                Disallow: /admin

                Sitemap: {baseUrl}/sitemap.xml
                """;
            return Results.Text(content, "text/plain", Encoding.UTF8);
        });

        return app;
    }
}
