using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;
using Portfolio.Web.Features.Admin.Messages.DeleteMessage;
using Portfolio.Web.Features.Admin.Messages.MarkMessageRead;

namespace Portfolio.Web.Pages.Admin.Messages;

public class IndexModel(
    AppDbContext db,
    MarkMessageReadHandler markReadHandler,
    DeleteMessageHandler deleteHandler) : PageModel
{
    public sealed record Row(int Id, string Name, string Email, string Message, DateTimeOffset CreatedAt, bool IsRead);

    public IReadOnlyList<Row> Messages { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        Messages = await db.ContactMessages
            .AsNoTracking()
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new Row(m.Id, m.Name, m.Email, m.Message, m.CreatedAt, m.IsRead))
            .ToListAsync(ct);
    }

    public async Task<IActionResult> OnPostToggleReadAsync(int id, CancellationToken ct)
    {
        var result = await markReadHandler.HandleAsync(id, ct);
        if (result.IsFailure)
            TempData["Message"] = result.Error!.Message;
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
    {
        var result = await deleteHandler.HandleAsync(id, ct);
        TempData["Message"] = result.IsSuccess ? "Mesaj silindi." : result.Error!.Message;
        return RedirectToPage();
    }
}
