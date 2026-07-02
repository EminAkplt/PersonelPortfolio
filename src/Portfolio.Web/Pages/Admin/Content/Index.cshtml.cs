using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;
using Portfolio.Web.Features.Admin.Content.UpdateContent;

namespace Portfolio.Web.Pages.Admin.Content;

public class IndexModel(AppDbContext db, UpdateContentHandler updateHandler) : PageModel
{
    [BindProperty]
    public Dictionary<string, string> Values { get; set; } = [];

    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        Values = await db.SiteContents
            .AsNoTracking()
            .ToDictionaryAsync(c => c.Key, c => c.Value, ct);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        var result = await updateHandler.HandleAsync(Values, ct);
        if (result.IsFailure)
        {
            ErrorMessage = result.Error!.Message;
            return Page();
        }

        TempData["Message"] = "İçerik güncellendi.";
        return RedirectToPage();
    }
}
