using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public class EngagementTaskEntityTypeConfiguration : BaseEntityTypeConfiguration<EngagementTask>
{
    public override void Configure(EntityTypeBuilder<EngagementTask> builder)
    {
        base.Configure(builder);

        builder.ToTable("EngagementTasks");

        builder.HasKey(engagementTask => engagementTask.Id);

        builder.Property(engagementTask => engagementTask.Id)
               .IsRequired()
               .HasConversion(
                    engagementTaskId => engagementTaskId.Value.ToString(),
                    value => EngagementTaskId.Create(value));
        
        builder.HasOne(engagementTask => engagementTask.Engagement)
               .WithMany(engagement => engagement.EngagementTasks)
               .HasForeignKey(engagementTask => engagementTask.EngagementId);
        
        builder.Property(engagementTask => engagementTask.EngagementId)
               .IsRequired()
               .HasConversion(
                    engagementId => engagementId.Value.ToString(),
                    value => EngagementId.Create(value));
        
        builder.Property(engagementTask => engagementTask.Title).IsRequired().HasMaxLength(100);
        builder.Property(engagementTask => engagementTask.Description).IsRequired(false).HasMaxLength(200);
        builder.Property(engagementTask => engagementTask.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
        builder.Property(engagementTask => engagementTask.DueDate).IsRequired();
        builder.Property(engagementTask => engagementTask.IsAssigned).IsRequired();
    }
}
