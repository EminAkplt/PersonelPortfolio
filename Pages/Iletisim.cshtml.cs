using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Web.Domain;
using Portfolio.Web.Features.Content.GetSiteContent;

namespace Portfolio.Web.Pages;

public class IletisimModel(GetSiteContentHandler contentHandler) : PageModel
{
    public string Email { get; private set; } = "";

    public async Task OnGetAsync(CancellationToken ct)
    {
        var content = await contentHandler.HandleAsync(ct);
        Email = content.GetValueOrDefault(SiteContentKeys.ContactEmail, "");
    }
}
