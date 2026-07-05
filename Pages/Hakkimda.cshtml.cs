using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Web.Domain;
using Portfolio.Web.Features.Content.GetSiteContent;
using Portfolio.Web.Features.Now.GetNow;

namespace Portfolio.Web.Pages;

public class HakkimdaModel(
    GetSiteContentHandler contentHandler,
    GetNowHandler nowHandler) : PageModel
{
    public IReadOnlyDictionary<string, string> Texts { get; private set; } = new Dictionary<string, string>();
    public NowResponse? Now { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
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
