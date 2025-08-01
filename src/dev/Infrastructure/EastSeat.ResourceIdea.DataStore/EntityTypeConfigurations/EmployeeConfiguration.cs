// =============================================================================
// File: EmployeeConfiguration.cs
// Path: e:\repos\resourceidea\resourceideaui\src\dev\Infrastructure\EastSeat.ResourceIdea.DataStore\EntityTypeConfigurations
// Description: Entity Framework Core configuration for the Employee entity
// This class configures the Employee entity mapping to the database including
// primary keys, foreign keys, and property conversions for value objects.
// =============================================================================

using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

/// <summary>
/// Entity Framework Core configuration for the Employee entity.
/// This class configures the Employee entity mapping to the database including
/// primary keys, foreign keys, and property conversions for value objects.
/// </summary>
public sealed class EmployeeConfiguration : BaseEntityConfiguration<Employee>
{
    public override void Configure(EntityTypeBuilder<Employee> builder)
    {
        base.Configure(builder);

        builder.ToTable("Employees");

        builder.HasKey(employee => employee.EmployeeId);

        builder.Property(employee => employee.EmployeeId)
            .IsRequired()
            .HasConversion(
                employeeId => employeeId.Value.ToString(),
                value => EmployeeId.Create(value));

        builder.Property(employee => employee.JobPositionId)
            .IsRequired()
            .HasConversion(
                jobPositionId => jobPositionId.Value.ToString(),
                value => JobPositionId.Create(value))
            .HasMaxLength(450);

        builder.Property(employee => employee.ApplicationUserId)
            .IsRequired()
            .HasConversion(
                applicationUserId => applicationUserId.Value.ToString(),
                value => ApplicationUserId.Create(value))
            .HasMaxLength(450);

        builder.Property(employee => employee.EmployeeNumber)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(employee => employee.ReportsTo)
            .HasConversion(
                managerId => managerId.Value.ToString(),
                value => EmployeeId.Create(value))
            .HasMaxLength(450);

        builder.Property(employee => employee.MigrationResourceId)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.HasOne(employee => employee.JobPosition)
            .WithMany(jobPosition => jobPosition.Employees)
            .HasForeignKey(employee => employee.JobPositionId);

        builder.Property(employee => employee.HireDate)
            .IsRequired(false);

        builder.Property(employee => employee.EndDate)
            .IsRequired(false);

        builder.Property(employee => employee.MigrationUserId)
            .IsRequired(false)
            .HasMaxLength(450);

        builder.Property(employee => employee.MigrationResourceId)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(employee => employee.MigrationFullname)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(employee => employee.MigrationCompanyCode)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(employee => employee.MigrationJobPositionId)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(employee => employee.MigrationJobsManagedColor)
            .IsRequired(false)
            .HasMaxLength(10);
    }
}