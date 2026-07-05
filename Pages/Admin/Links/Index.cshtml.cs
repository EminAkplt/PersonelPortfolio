using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;
using Portfolio.Web.Domain;
using Portfolio.Web.Features.Admin.Content.UpdateContent;
using Portfolio.Web.Features.Admin.Links.ManageCv;

namespace Portfolio.Web.Pages.Admin.Links;

public class IndexModel(
    AppDbContext db,
    UpdateContentHandler updateContentHandler,
    ManageCvHandler cvHandler) : PageModel
{
    [BindProperty]
    public string? Email { get; set; }

    [BindProperty]
    public string? Linkedin { get; set; }

    [BindProperty]
    public string? Github { get; set; }

    [BindProperty]
    public IFormFile? Upload { get; set; }

    public string? CvFile { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct) => await LoadAsync(ct);

    public async Task<IActionResult> OnPostLinksAsync(CancellationToken ct)
    {
        var values = new Dictionary<string, string>
        {
            [SiteContentKeys.ContactEmail] = Email?.Trim() ?? string.Empty,
            [SiteContentKeys.SocialLinkedin] = Linkedin?.Trim() ?? string.Empty,
            [SiteContentKeys.SocialGithub] = Github?.Trim() ?? string.Empty
        };

        var result = await updateContentHandler.HandleAsync(values, ct);
        if (result.IsFailure)
        {
            ErrorMessage = result.Error!.Message;
            await LoadAsync(ct);
            return Page();
        }

        TempData["Message"] = "Bağlantılar güncellendi.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCvAsync(CancellationToken ct)
    {
        var result = await cvHandler.UploadAsync(Upload, ct);
        if (result.IsFailure)
        {
            ErrorMessage = result.Error!.Message;
            await LoadAsync(ct);
            return Page();
        }

        TempData["Message"] = "CV yüklendi.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveCvAsync(CancellationToken ct)
    {
        await cvHandler.RemoveAsync(ct);
        TempData["Message"] = "CV kaldırıldı.";
        return RedirectToPage();
    }

    private async Task LoadAsync(CancellationToken ct)
    {
        var content = await db.SiteContents.AsNoTracking()
            .ToDictionaryAsync(c => c.Key, c => c.Value, ct);

        Email = content.GetValueOrDefault(SiteContentKeys.ContactEmail);
        Linkedin = content.GetValueOrDefault(SiteContentKeys.SocialLinkedin);
        Github = content.GetValueOrDefault(SiteContentKeys.SocialGithub);
        CvFile = content.GetValueOrDefault(SiteContentKeys.CvFile);
    }
}
