using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

/// <summary>
/// Represents the configuration for the <see cref="ApplicationRole"/> entity.
/// </summary>
public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.ToTable("Roles", "Identity");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.TenantId)
            .IsRequired(true)
            .HasMaxLength(256)
            .HasConversion(
                tenantId => tenantId.Value.ToString(),
                value => TenantId.Create(value));

        builder.Property(e => e.IsBackendRole).IsRequired(true);       
    }
}