using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public class EngagementConfiguration : BaseEntityConfiguration<Engagement>
{
    public override void Configure(EntityTypeBuilder<Engagement> builder)
    {
        base.Configure(builder);

        builder.ToTable("Engagements");

        builder.HasKey(engagement => engagement.Id);

        builder.Property(engagement => engagement.Id)
               .IsRequired()
               .HasConversion(
                    engagementId => engagementId.Value.ToString(),
                    value => EngagementId.Create(value));
        
        builder.Property(engagement => engagement.ClientId)
               .IsRequired()
               .HasConversion(
                    clientId => clientId.Value.ToString(),
                    value => ClientId.Create(value));

        builder.Property(engagement => engagement.Description).IsRequired().HasMaxLength(200);
        builder.Property(engagement => engagement.EngagementStatus).IsRequired().HasConversion<string>().HasMaxLength(50);
    }
}