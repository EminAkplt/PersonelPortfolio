using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Common.Caching;
using Portfolio.Web.Common.Http;
using Portfolio.Web.Common.Security;
using Portfolio.Web.Data;
using Portfolio.Web.Features;
using Portfolio.Web.Features.Seo;

// Üretim için şifre hash'i üretme komutu:
//   dotnet run --project src/Portfolio.Web -- hash-password "şifreniz"
if (args is ["hash-password", var password])
{
    Console.WriteLine(PasswordHasher.Hash(password));
    return;
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin");
    options.Conventions.AllowAnonymousToPage("/Admin/Login");
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/admin/giris";
        options.AccessDeniedPath = "/admin/giris";
        options.Cookie.Name = "portfolio.admin";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<AdminAuthService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IAppCache, MemoryAppCache>();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.AddFeatures();

var app = builder.Build();

await DbSeeder.InitializeAsync(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSecurityHeaders();
app.UseResponseCompression();

app.UseRouting();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapFeatureEndpoints();
app.MapSitemap();

app.Run();
