using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Web.Features.Projects.GetProjectBySlug;

namespace Portfolio.Web.Pages;

public class ProjeModel(GetProjectBySlugHandler handler) : PageModel
{
    public ProjectDetail Project { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(string slug, CancellationToken ct)
    {
        var result = await handler.HandleAsync(slug, ct);
        if (result.IsFailure)
            return NotFound();

        Project = result.Value;
        return Page();
    }
}
