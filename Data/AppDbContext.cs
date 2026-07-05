using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Domain;

namespace Portfolio.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<SiteContent> SiteContents => Set<SiteContent>();
    public DbSet<PageView> PageViews => Set<PageView>();
    public DbSet<NowStatus> NowStatuses => Set<NowStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(e =>
        {
            e.Property(p => p.Slug).HasMaxLength(120);
            e.HasIndex(p => p.Slug).IsUnique();
            e.Property(p => p.Title).HasMaxLength(200);
            e.Property(p => p.Summary).HasMaxLength(400);
            e.Property(p => p.CoverImageUrl).HasMaxLength(500);
            e.Property(p => p.DemoUrl).HasMaxLength(500);
            e.Property(p => p.RepoUrl).HasMaxLength(500);
            e.HasIndex(p => new { p.IsPublished, p.DisplayOrder });
        });

        modelBuilder.Entity<ContactMessage>(e =>
        {
            e.Property(m => m.Name).HasMaxLength(150);
            e.Property(m => m.Email).HasMaxLength(320);
            e.Property(m => m.Message).HasMaxLength(4000);
            e.Property(m => m.ClientIpHash).HasMaxLength(64);
            e.HasIndex(m => m.CreatedAt);
        });

        modelBuilder.Entity<SiteContent>(e =>
        {
            e.HasKey(c => c.Key);
            e.Property(c => c.Key).HasMaxLength(100);
        });

        modelBuilder.Entity<PageView>(e =>
        {
            e.Property(v => v.Path).HasMaxLength(300);
            e.Property(v => v.ClientIpHash).HasMaxLength(64);
            e.HasIndex(v => v.VisitedAt);
        });

        modelBuilder.Entity<NowStatus>(e =>
        {
            e.Property(s => s.StatusText).HasMaxLength(300);
            e.Property(s => s.Mood).HasConversion<string>().HasMaxLength(20);
        });
    }
}
