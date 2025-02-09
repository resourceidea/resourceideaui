using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public sealed class JobPositionConfiguration
    : BaseEntityConfiguration<JobPosition>
{
    public override void Configure(EntityTypeBuilder<JobPosition> builder)
    {
        base.Configure(builder);

        builder.ToTable("JobPositions");

        builder.HasKey(jobPosition => jobPosition.Id);

        builder.Property(jobPosition => jobPosition.Id)
            .IsRequired()
            .HasConversion(
                jobPositionId => jobPositionId.Value.ToString(),
                value => JobPositionId.Create(value));

        builder.Property(jobPosition => jobPosition.DepartmentId)
            .IsRequired()
            .HasConversion(
                departmentId => departmentId.Value.ToString(),
                value => DepartmentId.Create(value))
            .HasMaxLength(450);

        builder.Property(jobPosition => jobPosition.Name)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(jobPosition => jobPosition.Description)
               .HasMaxLength(500);
    }
}