using EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace EastSeat.ResourceIdea.DataStore;

public class ResourceIdeaDBContext(DbContextOptions<ResourceIdeaDBContext> options, IAuthenticationContext? authenticationContext = null)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    private readonly IAuthenticationContext? _authenticationContext = authenticationContext;

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<SubscriptionService> SubscriptionServices { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Engagement> Engagements { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<JobPosition> JobPositions { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<WorkItem> WorkItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserConfiguration());
        builder.ApplyConfiguration(new ApplicationRoleConfiguration());

        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Identity");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Identity");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Identity");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Identity");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Identity");

        builder.ApplyConfiguration(new TenantConfiguration());
        builder.ApplyConfiguration(new SubscriptionServiceConfiguration());
        builder.ApplyConfiguration(new SubscriptionConfiguration());
        builder.ApplyConfiguration(new ClientConfiguration());
        builder.ApplyConfiguration(new EngagementConfiguration());
        builder.ApplyConfiguration(new DepartmentConfiguration());
        builder.ApplyConfiguration(new JobPositionConfiguration());
        builder.ApplyConfiguration(new EmployeeConfiguration());
        builder.ApplyConfiguration(new WorkItemConfiguration());
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        var currentTime = DateTimeOffset.UtcNow;
        var currentUser = _authenticationContext?.ApplicationUserId.ToString() ?? "System";

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = currentTime;
                    entry.Entity.CreatedBy = currentUser;
                    entry.Entity.LastModified = currentTime;
                    entry.Entity.LastModifiedBy = currentUser;
                    
                    // Set TenantId if not already set and available from context
                    if (entry.Entity.TenantId == default && _authenticationContext?.TenantId != null && _authenticationContext.TenantId.IsNotEmpty())
                    {
                        entry.Entity.TenantId = _authenticationContext.TenantId;
                    }
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModified = currentTime;
                    entry.Entity.LastModifiedBy = currentUser;
                    break;

                case EntityState.Deleted:
                    // For soft delete
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.Deleted = currentTime;
                    entry.Entity.DeletedBy = currentUser;
                    entry.Entity.LastModified = currentTime;
                    entry.Entity.LastModifiedBy = currentUser;
                    break;
            }
        }
    }
}