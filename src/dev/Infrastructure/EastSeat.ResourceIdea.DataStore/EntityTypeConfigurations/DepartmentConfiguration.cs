using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public sealed class DepartmentConfiguration : BaseEntityTypeConfiguration<Department>
{
    public override void Configure(EntityTypeBuilder<Department> builder)
    {
        base.Configure(builder);

        builder.ToTable("Departments");

        builder.HasKey(department => department.Id);

        builder.Property(department => department.Id)
            .IsRequired()
            .HasConversion(
                departmentId => departmentId.Value.ToString(),
                value => DepartmentId.Create(value));

        builder.Property(department => department.Name).IsRequired().HasMaxLength(250);
    }
}
