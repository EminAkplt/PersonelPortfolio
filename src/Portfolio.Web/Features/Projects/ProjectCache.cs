using Portfolio.Web.Common.Caching;
using Portfolio.Web.Common.Localization;
using Portfolio.Web.Features.Projects.GetProjectBySlug;
using Portfolio.Web.Features.Projects.GetProjects;

namespace Portfolio.Web.Features.Projects;

/// <summary>Proje cache'ini tüm desteklenen diller için topluca temizler.</summary>
public static class ProjectCache
{
    public static void InvalidateAll(IAppCache cache, params string?[] slugs)
    {
        foreach (var culture in Loc.Supported)
        {
            cache.Remove(GetProjectsHandler.CacheKey(culture));

            foreach (var slug in slugs)
            {
                if (!string.IsNullOrEmpty(slug))
                    cache.Remove(GetProjectBySlugHandler.CacheKey(slug, culture));
            }
        }
    }
}
