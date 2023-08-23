using EastSeat.ResourceIdea.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.Persistence.Configurations;

/// <summary>
/// Employee table configuration.
/// </summary>
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(employee => employee.Id);

        builder.HasMany(employee => employee.Assignments)
            .WithOne(employeeAssignment => employeeAssignment.Employee)
            .HasForeignKey(assignment => assignment.EmployeeId);

        builder.ToTable("Employee");
    }
}
