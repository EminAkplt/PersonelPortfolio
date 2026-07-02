using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;
using Portfolio.Web.Features.Admin.Projects.SaveProject;

namespace Portfolio.Web.Pages.Admin.Projects;

public class EditModel(AppDbContext db, SaveProjectHandler saveHandler) : PageModel
{
    public sealed class ProjectForm
    {
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? Summary { get; set; }
        public string? Problem { get; set; }
        public string? Approach { get; set; }
        public string? Outcome { get; set; }
        public string? TechStackCsv { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? DemoUrl { get; set; }
        public string? RepoUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; }
    }

    [BindProperty]
    public ProjectForm Form { get; set; } = new();

    public bool IsNew { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task<IActionResult> OnGetAsync(int? id, CancellationToken ct)
    {
        IsNew = id is null or 0;
        if (IsNew)
        {
            Form.DisplayOrder = await db.Projects.CountAsync(ct) + 1;
            return Page();
        }

        var project = await db.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
        if (project is null)
            return RedirectToPage("/Admin/Projects/Index");

        Form = new ProjectForm
        {
            Title = project.Title,
            Slug = project.Slug,
            Summary = project.Summary,
            Problem = project.Problem,
            Approach = project.Approach,
            Outcome = project.Outcome,
            TechStackCsv = string.Join(", ", project.TechStack),
            CoverImageUrl = project.CoverImageUrl,
            DemoUrl = project.DemoUrl,
            RepoUrl = project.RepoUrl,
            DisplayOrder = project.DisplayOrder,
            IsPublished = project.IsPublished
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id, CancellationToken ct)
    {
        IsNew = id is null or 0;

        var techStack = (Form.TechStackCsv ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToArray();

        var request = new SaveProjectRequest(
            id, Form.Slug?.Trim(), Form.Title?.Trim(), Form.Summary?.Trim(),
            Form.Problem?.Trim(), Form.Approach?.Trim(), Form.Outcome?.Trim(),
            techStack, Form.CoverImageUrl, Form.DemoUrl, Form.RepoUrl,
            Form.DisplayOrder, Form.IsPublished);

        var result = await saveHandler.HandleAsync(request, ct);
        if (result.IsFailure)
        {
            ErrorMessage = result.Error!.Message;
            return Page();
        }

        TempData["Message"] = IsNew ? "Proje eklendi." : "Proje güncellendi.";
        return RedirectToPage("/Admin/Projects/Index");
    }
}
