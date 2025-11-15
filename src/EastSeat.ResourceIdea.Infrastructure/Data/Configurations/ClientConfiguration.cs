using EastSeat.ResourceIdea.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.Infrastructure.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.RegistrationNumber)
            .HasMaxLength(100);

        builder.Property(c => c.Sector)
            .HasMaxLength(100);

        builder.Property(c => c.ContactEmail)
            .HasMaxLength(200);

        builder.Property(c => c.ContactPhone)
            .HasMaxLength(50);

        builder.HasIndex(c => c.Name);
        builder.HasIndex(c => c.RegistrationNumber);
    }
}
