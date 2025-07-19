// --------------------------------------------------------------------------------------
// File: JobPositionConfiguration.cs
// Path: src\dev\Infrastructure\EastSeat.ResourceIdea.DataStore\EntityTypeConfigurations\JobPositionConfiguration.cs
// Description: Entity type configuration for JobPosition entity.
// --------------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

/// <summary>
/// Entity type configuration for JobPosition entity.
/// </summary>
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

        builder.Property(jobPosition => jobPosition.Title)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(jobPosition => jobPosition.Description)
               .HasMaxLength(500);

        builder.Property(jobPosition => jobPosition.MigrationJobPositionId)
               .IsRequired(false)
               .HasMaxLength(50);

        builder.Property(jobPosition => jobPosition.MigrationJobLevel)
                .IsRequired(false)
                .HasMaxLength(50);

        builder.Property(jobPosition => jobPosition.MigrationCompanyCode)
                .IsRequired(false)
                .HasMaxLength(50);
    }
}