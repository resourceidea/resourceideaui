using EastSeat.ResourceIdea.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.Persistence.Configurations;

/// <summary>
/// Subscription table configuration.
/// </summary>
public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(subscription => subscription.SubscriptionId);

        builder.Property(subscription => subscription.Status)
            .IsRequired()
            .HasDefaultValue(Constants.Subscription.Status.Active)
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

        builder.HasMany(subscription => subscription.Engagements)
            .WithOne(engagement => engagement.Subscription)
            .HasForeignKey(engagement => engagement.SubscriptionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(subscription => subscription.Assignments)
            .WithOne(assignment => assignment.Subscription)
            .HasForeignKey(assignment => assignment.SubscriptionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(subscription => subscription.Assets)
            .WithOne(asset => asset.Subscription)
            .HasForeignKey(asset => asset.SubscriptionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(subscription => subscription.Employees)
            .WithOne(employee => employee.Subscription)
            .HasForeignKey(employee => employee.SubscriptionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(subscription => subscription.JobPositions)
            .WithOne(jobPosition => jobPosition.Subscription)
            .HasForeignKey(jobPosition => jobPosition.SubscriptionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.ToTable("Subscription");
    }
}
