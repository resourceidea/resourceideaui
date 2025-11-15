using EastSeat.ResourceIdea.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.Infrastructure.Data.Configurations;

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.HasKey(lr => lr.Id);

        builder.Property(lr => lr.Reason)
            .HasMaxLength(500);

        builder.Property(lr => lr.WorkingDaysImpact)
            .HasPrecision(5, 2);

        builder.HasOne(lr => lr.Employee)
            .WithMany(e => e.LeaveRequests)
            .HasForeignKey(lr => lr.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(lr => lr.Approver)
            .WithMany()
            .HasForeignKey(lr => lr.ApprovedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(lr => new { lr.EmployeeId, lr.StartDate, lr.EndDate });
        builder.HasIndex(lr => lr.Status);
    }
}
