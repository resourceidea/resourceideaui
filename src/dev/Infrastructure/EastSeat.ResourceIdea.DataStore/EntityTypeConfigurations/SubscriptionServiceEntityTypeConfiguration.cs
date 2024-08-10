using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public class SubscriptionServiceEntityTypeConfiguration : BaseEntityTypeConfiguration<SubscriptionService>
{
    public override void Configure(EntityTypeBuilder<SubscriptionService> builder)
    {
        base.Configure(builder);

        builder.Ignore(subscriptionService => subscriptionService.TenantId);

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
    }
}
