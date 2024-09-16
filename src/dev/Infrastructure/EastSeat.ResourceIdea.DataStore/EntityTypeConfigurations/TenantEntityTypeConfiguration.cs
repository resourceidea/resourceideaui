using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public class TenantEntityTypeConfiguration : BaseEntityTypeConfiguration<Tenant>
{
    public override void Configure(EntityTypeBuilder<Tenant> builder)
    {
        base.Configure(builder);

        builder.ToTable("Tenants");

        builder.HasKey(tenant => tenant.TenantId)
               .HasName("PK_Tenants")
               .IsClustered(false);

        builder.Property(tenant => tenant.TenantId)
               .IsRequired()
               .HasConversion(
                    tenantId => tenantId.Value.ToString(),
                    value => TenantId.Create(value))
               .HasColumnType("nvarchar(450)");

        builder.HasKey(tenant => tenant.TenantId)
               .HasName("PK_Tenants")
               .IsClustered(false);

        builder.Property(tenant => tenant.Organization)
            .IsRequired()
            .HasMaxLength(500);
    }
}