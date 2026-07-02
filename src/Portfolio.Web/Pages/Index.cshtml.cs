using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Web.Domain;
using Portfolio.Web.Features.Content.GetSiteContent;
using Portfolio.Web.Features.Now.GetNow;
using Portfolio.Web.Features.Projects.GetProjects;

namespace Portfolio.Web.Pages;

public class IndexModel(
    GetProjectsHandler projectsHandler,
    GetSiteContentHandler contentHandler,
    GetNowHandler nowHandler) : PageModel
{
    public IReadOnlyList<ProjectListItem> Projects { get; private set; } = [];
    public IReadOnlyDictionary<string, string> Texts { get; private set; } = new Dictionary<string, string>();
    public NowResponse? Now { get; private set; }

    public string ContentOf(string key, string fallback = "") =>
        Texts.GetValueOrDefault(key, fallback);

    public async Task OnGetAsync(CancellationToken ct)
    {
        var projectsResult = await projectsHandler.HandleAsync(ct);
        Projects = projectsResult.Value;

        Texts = await contentHandler.HandleAsync(ct);

        var nowResult = await nowHandler.HandleAsync(ct);
        Now = nowResult.IsSuccess ? nowResult.Value : null;
    }

    public static string MoodEmoji(string mood) => mood switch
    {
        nameof(Mood.Building) => "🔨",
        nameof(Mood.Learning) => "📚",
        nameof(Mood.Shipping) => "🚀",
        _ => "🔨"
    };
}
