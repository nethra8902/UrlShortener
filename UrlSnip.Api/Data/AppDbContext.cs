using Microsoft.EntityFrameworkCore;
using UrlSnip.Api.Domain;

namespace UrlSnip.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ShortUrl> ShortUrls => Set<ShortUrl>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortUrl>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.Property(e => e.OriginalUrl).IsRequired();
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(20);
        });
    }
}
