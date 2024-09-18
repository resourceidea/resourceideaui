using EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace EastSeat.ResourceIdea.DataStore;

public class ResourceIdeaDBContext(DbContextOptions<ResourceIdeaDBContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Tenant>? Tenants { get; set; }
    public DbSet<SubscriptionService>? SubscriptionServices { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserEntityTypeConfiguration());
        builder.Entity<IdentityRole>().ToTable("Roles", "Identity");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Identity");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Identity");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Identity");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Identity");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Identity");

        builder.ApplyConfiguration(new TenantEntityTypeConfiguration());
        builder.ApplyConfiguration(new SubscriptionServiceEntityTypeConfiguration());
        builder.ApplyConfiguration(new SubscriptionEntityTypeConfiguration());
        builder.ApplyConfiguration(new ClientEntityTypeConfiguration());
        builder.ApplyConfiguration(new EngagementEntityTypeConfiguration());
        builder.ApplyConfiguration(new EngagementTaskEntityTypeConfiguration());
        builder.ApplyConfiguration(new EngagementTaskAssignmentEntityTypeConfiguration());
    }
}
