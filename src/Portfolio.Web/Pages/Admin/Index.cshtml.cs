using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Web.Features.Admin.Dashboard.GetDashboard;

namespace Portfolio.Web.Pages.Admin;

public class IndexModel(GetDashboardHandler dashboardHandler) : PageModel
{
    public DashboardData Dashboard { get; private set; } = null!;

    public async Task OnGetAsync(CancellationToken ct)
    {
        var result = await dashboardHandler.HandleAsync(ct);
        Dashboard = result.Value;
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("/Admin/Login");
    }
}
