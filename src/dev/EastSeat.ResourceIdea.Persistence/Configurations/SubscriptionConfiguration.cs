using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Persistence.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.Persistence.Configurations;

/// <summary>
/// Subscription table configuration.
/// </summary>
public class SubscriptionConfiguration : IEntityTypeConfiguration<SubscriptionEntity>
{
    public void Configure(EntityTypeBuilder<SubscriptionEntity> builder)
    {
        builder.HasKey(subscription => subscription.SubscriptionId);

        builder.Property(subscription => subscription.SubscriptionId)
            .ValueGeneratedOnAdd();

        builder.Property(subscription => subscription.Status)
            .IsRequired()
            .HasDefaultValue(SubscriptionStatus.Active)
            .HasConversion<string>();

        builder.Property(subscription => subscription.SubscriberName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(subscription => subscription.StartDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasMany(subscription => subscription.Clients)
            .WithOne(client => client.Subscription)
            .HasForeignKey(client => client.SubscriptionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.ToTable("Subscription");
    }
}
