using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public class SubscriptionEntityTypeConfiguration : BaseEntityTypeConfiguration<Subscription>
{
    public override void Configure(EntityTypeBuilder<Subscription> builder)
    {
        base.Configure(builder);

        builder.ToTable("Subscriptions");

        builder.HasKey(subscription => subscription.Id);

        builder.Property(subscription => subscription.Id)
            .IsRequired()
            .HasConversion(
                subscriptionId => subscriptionId.Value.ToString(),
                value => SubscriptionId.Create(value));

        builder.Property(Subscription => Subscription.SubscriptionServiceId)
            .IsRequired()
            .HasConversion(
                subscriptionServiceId => subscriptionServiceId.Value.ToString(),
                value => SubscriptionServiceId.Create(value));
    }
}
