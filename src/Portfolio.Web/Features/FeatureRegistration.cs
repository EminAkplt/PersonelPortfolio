using System.Threading.RateLimiting;
using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;
using Portfolio.Web.Common.Security;
using Portfolio.Web.Features.Admin.Content.UpdateContent;
using Portfolio.Web.Features.Admin.Dashboard.GetDashboard;
using Portfolio.Web.Features.Admin.Links.ManageCv;
using Portfolio.Web.Features.Admin.Messages.DeleteMessage;
using Portfolio.Web.Features.Admin.Messages.MarkMessageRead;
using Portfolio.Web.Features.Admin.Now.UpdateNow;
using Portfolio.Web.Features.Admin.Projects.DeleteProject;
using Portfolio.Web.Features.Admin.Projects.SaveProject;
using Portfolio.Web.Features.Admin.Projects.TogglePublish;
using Portfolio.Web.Features.Contact.SubmitContact;
using Portfolio.Web.Features.Content.GetSiteContent;
using Portfolio.Web.Features.Health;
using Portfolio.Web.Features.Now.GetNow;
using Portfolio.Web.Features.Projects.GetProjectBySlug;
using Portfolio.Web.Features.Projects.GetProjects;
using Portfolio.Web.Features.Tracking.TrackPageView;

namespace Portfolio.Web.Features;

public static class FeatureRegistration
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        services.AddSingleton<IpHasher>();
        services.AddValidatorsFromAssemblyContaining<SubmitContactValidator>();

        services.AddScoped<GetProjectsHandler>();
        services.AddScoped<GetProjectBySlugHandler>();
        services.AddScoped<SubmitContactHandler>();
        services.AddScoped<GetNowHandler>();
        services.AddScoped<TrackPageViewHandler>();
        services.AddScoped<GetSiteContentHandler>();

        services.AddScoped<SaveProjectHandler>();
        services.AddScoped<DeleteProjectHandler>();
        services.AddScoped<TogglePublishHandler>();
        services.AddScoped<MarkMessageReadHandler>();
        services.AddScoped<DeleteMessageHandler>();
        services.AddScoped<UpdateContentHandler>();
        services.AddScoped<UpdateNowHandler>();
        services.AddScoped<GetDashboardHandler>();
        services.AddScoped<ManageCvHandler>();

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (context, ct) =>
            {
                context.HttpContext.Response.ContentType = "application/json; charset=utf-8";
                await context.HttpContext.Response.WriteAsync(
                    """{"message":"Kısa sürede çok fazla mesaj gönderdiniz. Bir saat sonra tekrar deneyebilirsiniz."}""", ct);
            };

            options.AddPolicy(SubmitContactEndpoint.RateLimitPolicy, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromHours(1),
                        QueueLimit = 0
                    }));
        });

        return services;
    }

    public static IEndpointRouteBuilder MapFeatureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetProjects();
        app.MapGetProjectBySlug();
        app.MapSubmitContact();
        app.MapGetNow();
        app.MapTrackPageView();
        app.MapHealth();

        return app;
    }
}
