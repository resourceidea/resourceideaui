using EastSeat.ResourceIdea.Domain.Common;
using EastSeat.ResourceIdea.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.Persistence;

/// <summary>
/// App database context.
/// </summary>
public class ResourceIdeaDbContext : DbContext
{
    public ResourceIdeaDbContext(DbContextOptions<ResourceIdeaDbContext> options) : base(options)
    {
    }

    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Engagement> Engagements { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<AssetAssignment> AssetAssignments { get; set; }
    public DbSet<EmployeeAssignment> EmployeeAssignments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<JobPosition> JobPositions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResourceIdeaDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseSubscriptionEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
