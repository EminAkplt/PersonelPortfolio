using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;
using Portfolio.Web.Domain;
using Portfolio.Web.Features.Admin.Now.UpdateNow;

namespace Portfolio.Web.Pages.Admin.Now;

public class IndexModel(AppDbContext db, UpdateNowHandler updateHandler) : PageModel
{
    [BindProperty]
    public string? StatusText { get; set; }

    [BindProperty]
    public Mood Mood { get; set; } = Mood.Building;

    public DateTimeOffset? LastUpdated { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        var status = await db.NowStatuses.AsNoTracking()
            .OrderByDescending(s => s.UpdatedAt)
            .FirstOrDefaultAsync(ct);

        if (status is not null)
        {
            StatusText = status.StatusText;
            Mood = status.Mood;
            LastUpdated = status.UpdatedAt;
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        var result = await updateHandler.HandleAsync(StatusText, Mood, ct);
        if (result.IsFailure)
        {
            ErrorMessage = result.Error!.Message;
            return Page();
        }

        TempData["Message"] = "Durum güncellendi.";
        return RedirectToPage();
    }
}
