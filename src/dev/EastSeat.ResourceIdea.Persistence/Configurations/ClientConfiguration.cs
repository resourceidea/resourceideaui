using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Persistence.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.Persistence.Configurations;

/// <summary>
/// Client table configuration.
/// </summary>
public class ClientConfiguration : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.HasKey(client => client.Id);

        builder.Property(client => client.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(client => client.Address)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(client => client.ColorCode)
            .HasMaxLength(6)
            .IsRequired(false);

        builder.ToTable("Client");
    }
}
