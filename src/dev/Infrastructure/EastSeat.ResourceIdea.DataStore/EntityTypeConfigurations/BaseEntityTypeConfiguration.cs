using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EastSeat.ResourceIdea.Domain.Common.Entities;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(tenant => tenant.TenantId);

        builder.Property(tenant => tenant.Created)
            .IsRequired();

        builder.Property(tenant => tenant.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(tenant => tenant.LastModified)
            .IsRequired();

        builder.Property(tenant => tenant.LastModifiedBy)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(tenant => tenant.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(tenant => tenant.Deleted)
            .IsRequired(false);

        builder.Property(tenant => tenant.DeletedBy)
            .IsRequired(false)
            .HasMaxLength(100);
    }
}
