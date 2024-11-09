using EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace EastSeat.ResourceIdea.DataStore;

public class ResourceIdeaDBContext(DbContextOptions<ResourceIdeaDBContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<SubscriptionService> SubscriptionServices { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Engagement> Engagements { get; set; }
    public DbSet<EngagementTask> EngagementTasks { get; set; }
    public DbSet<EngagementTaskAssignment> EngagementTaskAssignments { get; set; }
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable("Roles", "Identity");

            entity.Property(e => e.TenantId)
            .IsRequired(true)
            .HasMaxLength(256)
            .HasConversion(
                tenantId => tenantId.Value.ToString(),
                value => TenantId.Create(value));

            entity.Property(e => e.IsBackendRole).IsRequired(true);

            entity.Property(e => e.Name).IsRequired(true).HasMaxLength(256);

            entity.Property(e => e.NormalizedName).IsRequired(false).HasMaxLength(256);
        });

        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Identity");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Identity");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Identity");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Identity");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Identity");
        builder.Entity<IdentityRole<string>>().ToTable("ApplicationRole", "Identity");

        builder.ApplyConfiguration(new ApplicationUserEntityTypeConfiguration());
        builder.ApplyConfiguration(new TenantEntityTypeConfiguration());
        builder.ApplyConfiguration(new SubscriptionServiceEntityTypeConfiguration());
        builder.ApplyConfiguration(new SubscriptionEntityTypeConfiguration());
        builder.ApplyConfiguration(new ClientEntityTypeConfiguration());
        builder.ApplyConfiguration(new EngagementEntityTypeConfiguration());
        builder.ApplyConfiguration(new EngagementTaskEntityTypeConfiguration());
        builder.ApplyConfiguration(new EngagementTaskAssignmentEntityTypeConfiguration());
    }
}
