using EastSeat.ResourceIdea.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.Infrastructure.Data.Configurations;

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.PlannedHours)
            .HasPrecision(10, 2);

        builder.Property(a => a.ColorHint)
            .HasMaxLength(20);

        builder.HasOne(a => a.EngagementYear)
            .WithMany(ey => ey.Assignments)
            .HasForeignKey(a => a.EngagementYearId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Employee)
            .WithMany(e => e.Assignments)
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => new { a.EmployeeId, a.StartDate, a.EndDate });
        builder.HasIndex(a => new { a.EngagementYearId, a.EmployeeId });
    }
}
