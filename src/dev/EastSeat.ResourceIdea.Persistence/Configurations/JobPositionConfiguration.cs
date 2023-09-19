using EastSeat.ResourceIdea.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.Persistence.Configurations;

/// <summary>
/// JobPosition table configuration.
/// </summary>
public class JobPositionConfiguration : IEntityTypeConfiguration<JobPosition>
{
    public void Configure(EntityTypeBuilder<JobPosition> builder)
    {
        builder.HasKey(jobPosition => jobPosition.Id);

        builder.HasMany(jobPosition => jobPosition.Employees)
            .WithOne(employee => employee.JobPosition)
            .HasForeignKey(employee => employee.JobPositionId);

        builder.Property(jobPosition => jobPosition.Description)
            .HasMaxLength(250);

        builder.Property(jobPosition => jobPosition.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.ToTable("JobPosition");
    }
}
