using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Web.Features.Content.GetSiteContent;

namespace Portfolio.Web.Pages;

public class IndexModel(GetSiteContentHandler contentHandler) : PageModel
{
    public IReadOnlyDictionary<string, string> Texts { get; private set; } = new Dictionary<string, string>();

    public async Task OnGetAsync(CancellationToken ct)
    {
        Texts = await contentHandler.HandleAsync(ct);
    }
}
