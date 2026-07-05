using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;
using Portfolio.Web.Features.Admin.Projects.DeleteProject;
using Portfolio.Web.Features.Admin.Projects.TogglePublish;

namespace Portfolio.Web.Pages.Admin.Projects;

public class IndexModel(
    AppDbContext db,
    TogglePublishHandler togglePublishHandler,
    DeleteProjectHandler deleteProjectHandler) : PageModel
{
    public sealed record Row(int Id, string Title, string Slug, int DisplayOrder, bool IsPublished, DateTimeOffset UpdatedAt);

    public IReadOnlyList<Row> Projects { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        Projects = await db.Projects
            .AsNoTracking()
            .OrderBy(p => p.DisplayOrder)
            .Select(p => new Row(p.Id, p.Title, p.Slug, p.DisplayOrder, p.IsPublished, p.UpdatedAt))
            .ToListAsync(ct);
    }

    public async Task<IActionResult> OnPostTogglePublishAsync(int id, CancellationToken ct)
    {
        var result = await togglePublishHandler.HandleAsync(id, ct);
        TempData["Message"] = result.IsSuccess
            ? (result.Value ? "Proje yayına alındı." : "Proje yayından kaldırıldı.")
            : result.Error!.Message;
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
    {
        var result = await deleteProjectHandler.HandleAsync(id, ct);
        TempData["Message"] = result.IsSuccess ? "Proje silindi." : result.Error!.Message;
        return RedirectToPage();
    }
}
