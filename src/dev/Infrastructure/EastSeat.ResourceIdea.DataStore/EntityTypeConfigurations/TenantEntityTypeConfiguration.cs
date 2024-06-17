using EastSeat.ResourceIdea.Domain.Tenants.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public class TenantEntityTypeConfiguration : BaseEntityTypeConfiguration<Tenant>
{
    public override void Configure(EntityTypeBuilder<Tenant> builder)
    {
        base.Configure(builder);

        builder.ToTable("Tenants");

        builder.HasKey(tenant => tenant.TenantId);

        builder.Property(tenant => tenant.Organization)
            .IsRequired()
            .HasMaxLength(500);
    }
}