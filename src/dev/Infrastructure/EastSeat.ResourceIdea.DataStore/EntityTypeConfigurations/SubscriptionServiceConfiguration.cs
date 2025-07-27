using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public class SubscriptionServiceConfiguration : IEntityTypeConfiguration<SubscriptionService>
{
    public void Configure(EntityTypeBuilder<SubscriptionService> builder)
    {
        builder.ToTable("SubscriptionServices");

        builder.HasKey(subscriptionService => subscriptionService.Id);

        builder.Property(subscription => subscription.Id)
               .IsRequired()
               .HasConversion(
                    subscriptionServiceId => subscriptionServiceId.Value.ToString(),
                    value => SubscriptionServiceId.Create(value));

        builder.Property(subscriptionService => subscriptionService.Name)
            .IsRequired()
            .HasMaxLength(500);

        // Configure BaseEntity properties manually (excluding TenantId)
        builder.Property(subscriptionService => subscriptionService.Created)
            .IsRequired();

        builder.Property(subscriptionService => subscriptionService.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(subscriptionService => subscriptionService.LastModified)
            .IsRequired();

        builder.Property(subscriptionService => subscriptionService.LastModifiedBy)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(subscriptionService => subscriptionService.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(subscriptionService => subscriptionService.Deleted)
            .IsRequired(false);

        builder.Property(subscriptionService => subscriptionService.DeletedBy)
            .IsRequired(false)
            .HasMaxLength(100);

        // Ignore TenantId since SubscriptionService is a global entity
        builder.Ignore(subscriptionService => subscriptionService.TenantId);
    }
}
