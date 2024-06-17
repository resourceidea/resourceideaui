using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.DataStore.EntityTypeConfigurations;

public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable(name: "ApplicationUsers", schema: "Identity");

        builder.HasKey(applicationUser => applicationUser.Id);

        builder.Property(applicationUser => applicationUser.ApplicationUserId)
               .IsRequired()
               .HasConversion(
                    applicationUserId => applicationUserId.Value.ToString(),
                    value => ApplicationUserId.Create(value));

        builder.Property(applicationUser => applicationUser.TenantId)
                .IsRequired()
                .HasConversion(
                    tenantId => tenantId.Value.ToString(),
                    value => TenantId.Create(value));

        builder.Property(applicationUser => applicationUser.FirstName)
                .IsRequired()
                .HasMaxLength(100);

        builder.Property(applicationUser => applicationUser.LastName)
                .IsRequired()
                .HasMaxLength(100);
    }
}
