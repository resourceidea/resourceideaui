using EastSeat.ResourceIdea.Application.Common;
using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.Infrastructure.Data;

/// <summary>
/// Main database context for the application
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Domain entities
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Engagement> Engagements => Set<Engagement>();
    public DbSet<EngagementYear> EngagementYears => Set<EngagementYear>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
    public DbSet<PublicHoliday> PublicHolidays => Set<PublicHoliday>();
    public DbSet<RolloverHistory> RolloverHistories => Set<RolloverHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Customize Identity table names
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");
        });

        modelBuilder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable("UserRoles");
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Set audit fields for entities implementing AuditableEntity
        // This would be implemented with proper user context

        return await base.SaveChangesAsync(cancellationToken);
    }
}
