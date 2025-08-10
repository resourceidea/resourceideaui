using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public class WorkItemConfiguration : BaseEntityConfiguration<WorkItem>
{
    public override void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("WorkItems");

        builder.HasKey(workItem => workItem.Id);

        builder.Property(workItem => workItem.Id)
            .IsRequired()
            .HasConversion(
                workItemId => workItemId.Value.ToString(),
                value => WorkItemId.Create(value));

        builder.Property(workItem => workItem.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(workItem => workItem.Description)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(workItem => workItem.EngagementId)
            .IsRequired()
            .HasConversion(
                engagementId => engagementId.Value.ToString(),
                value => EngagementId.Create(value));

        builder.Property(workItem => workItem.PlannedStartDate)
            .IsRequired(false);

        builder.Property(workItem => workItem.CompletedDate)
            .IsRequired(false);

        builder.Property(workItem => workItem.PlannedEndDate)
            .IsRequired(false);

        builder.Property(workItem => workItem.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(workItem => workItem.Priority)
            .IsRequired();

        builder.Property(workItem => workItem.AssignedToId)
            .IsRequired(false)
            .HasConversion(
                employeeId => employeeId == null ? null : employeeId.Value.ToString(),
                value => string.IsNullOrEmpty(value) ? null : EmployeeId.Create(value));

        builder.Property(workItem => workItem.MigrationJobId)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(workItem => workItem.MigrationJobResourceId)
            .IsRequired(false)
            .HasMaxLength(100);
        
        builder.Property(workItem => workItem.MigrationResourceId)
            .IsRequired(false)
            .HasMaxLength(100);
    }
}
