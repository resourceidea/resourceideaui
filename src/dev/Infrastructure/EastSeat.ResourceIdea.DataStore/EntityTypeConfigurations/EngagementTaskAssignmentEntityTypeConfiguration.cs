using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public class EngagementTaskAssignmentEntityTypeConfiguration : BaseEntityTypeConfiguration<EngagementTaskAssignment>
{
    public override void Configure(EntityTypeBuilder<EngagementTaskAssignment> builder)
    {
        base.Configure(builder);

        builder.ToTable("EngagementTaskAssignments");

        builder.HasKey(engagementTaskAssignment => engagementTaskAssignment.Id);
        
        builder.HasOne(engagementTaskAssignment => engagementTaskAssignment.EngagementTask)
               .WithMany(engagementTask => engagementTask.EngagementTaskAssignments)
               .HasForeignKey(engagementTaskAssignment => engagementTaskAssignment.EngagementTaskId);
        
        builder.Property(engagementTaskAssignment => engagementTaskAssignment.EngagementTaskId)
               .IsRequired()
               .HasConversion(
                    engagementTaskId => engagementTaskId.Value.ToString(),
                    value => EngagementTaskId.Create(value));
        
        builder.Property(engagementTaskAssignment => engagementTaskAssignment.ApplicationUserId)
                .IsRequired()
                .HasConversion(
                      applicationUserId => applicationUserId.Value,
                      value => ApplicationUserId.Create(value));
        
        builder.Property(engagementTaskAssignment => engagementTaskAssignment.StartDate).IsRequired();
        builder.Property(engagementTaskAssignment => engagementTaskAssignment.EndDate).IsRequired();
        builder.Property(engagementTaskAssignment => engagementTaskAssignment.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
    }
}
