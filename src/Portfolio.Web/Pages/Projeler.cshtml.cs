using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Web.Features.Projects.GetProjects;

namespace Portfolio.Web.Pages;

public class ProjelerModel(GetProjectsHandler projectsHandler) : PageModel
{
    public IReadOnlyList<ProjectListItem> Projects { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        var result = await projectsHandler.HandleAsync(ct);
        Projects = result.Value;
    }
}
