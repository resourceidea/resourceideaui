using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public class DepartmentEntityTypeConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable(name: "Departments");

        builder.HasKey(department => department.DepartmentId);

        builder.Property(department => department.DepartmentId)
               .IsRequired()
               .HasConversion(
                    departmentId => departmentId.Value.ToString(),
                    value => DepartmentId.Create(value));

        builder.Property(department => department.TenantId)
                .IsRequired()
                .HasConversion(
                    tenantId => tenantId.Value.ToString(),
                    value => TenantId.Create(value));

        builder.Property(department => department.DepartmentName)
                .IsRequired()
                .HasMaxLength(200);
    }
}
