using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EastSeat.ResourceIdea.Shared.Models;

namespace EastSeat.ResourceIdea.Server.Data;

public sealed class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbSet<Client> Clients => Set<Client>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Client>(cfg =>
        {
            cfg.HasKey(c => c.Id);
            cfg.Property(c => c.Name).IsRequired().HasMaxLength(200);
            cfg.Property(c => c.Code).IsRequired().HasMaxLength(50);
            cfg.Property(c => c.Status).HasConversion<int>();
            cfg.HasIndex(c => c.Code).IsUnique();
        });
    }
}
