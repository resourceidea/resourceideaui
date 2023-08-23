using EastSeat.ResourceIdea.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.Persistence.Configurations;

/// <summary>
/// Engagement table configuration.
/// </summary>
public class EngagementConfiguration : IEntityTypeConfiguration<Engagement>
{
    public void Configure(EntityTypeBuilder<Engagement> builder)
    {
        builder.HasKey(engagement => engagement.Id);

        builder.HasMany(engagement => engagement.Assignments)
            .WithOne(assignment => assignment.Engagement)
            .HasForeignKey(assignment => assignment.EngagementId);

        builder.ToTable("Engagement");
    }
}
