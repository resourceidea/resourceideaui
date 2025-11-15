using EastSeat.ResourceIdea.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.Infrastructure.Data.Configurations;

public class EngagementYearConfiguration : IEntityTypeConfiguration<EngagementYear>
{
    public void Configure(EntityTypeBuilder<EngagementYear> builder)
    {
        builder.HasKey(ey => ey.Id);

        builder.Property(ey => ey.CarryForwardHours)
            .HasPrecision(10, 2);

        builder.HasOne(ey => ey.Engagement)
            .WithMany(e => e.EngagementYears)
            .HasForeignKey(ey => ey.EngagementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ey => new { ey.EngagementId, ey.Year }).IsUnique();
    }
}
