using EastSeat.ResourceIdea.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.Persistence.Configurations;

/// <summary>
/// Client table configuration.
/// </summary>
public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(client => client.Id);

        builder.HasMany(client => client.Engagements)
            .WithOne(engagement => engagement.Client)
            .HasForeignKey(engagement => engagement.ClientId);

        builder.Property(client => client.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(client => client.Address)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.ToTable("Client");
    }
}
