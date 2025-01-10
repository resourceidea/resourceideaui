using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public class ClientConfiguration : BaseEntityConfiguration<Client>
{
    public override void Configure(EntityTypeBuilder<Client> builder)
    {
        base.Configure(builder);

        builder.ToTable("Clients");

        builder.HasKey(client => client.Id);

        builder.Property(client => client.Id)
               .IsRequired()
               .HasConversion(
                    clientId => clientId.Value.ToString(),
                    value => ClientId.Create(value));

        builder.Property(client => client.Name).IsRequired().HasMaxLength(500);

        builder.OwnsOne(client => client.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Building).IsRequired().HasMaxLength(100);
            addressBuilder.Property(a => a.Street).IsRequired().HasMaxLength(100);
            addressBuilder.Property(a => a.City).IsRequired().HasMaxLength(100);
        });
    }
}
