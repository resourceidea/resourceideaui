using EastSeat.ResourceIdea.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.Persistence.Configurations;

/// <summary>
/// Asset table configuration.
/// </summary>
public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.HasKey(asset => asset.Id);

        builder.HasMany(asset => asset.Assignments)
            .WithOne(assetAssignment => assetAssignment.Asset)
            .HasForeignKey(assetAssignment => assetAssignment.AssetId);

        builder.Property(asset => asset.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Property(asset => asset.Description)
            .HasMaxLength(250);

        builder.ToTable("Asset");
    }
}
