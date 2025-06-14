using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
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

builder.Property(engagement => engagement.ManagerId)
       .IsRequired(false)
       .HasConversion(
            employeeId => employeeId?.Value.ToString(),
            value        => string.IsNullOrEmpty(value) ? null : EmployeeId.Create(value));

builder.Property(engagement => engagement.PartnerId)
       .IsRequired(false)
       .HasConversion(
            employeeId => employeeId?.Value.ToString(),
            value        => string.IsNullOrEmpty(value) ? null : EmployeeId.Create(value));

builder.Property(engagement => engagement.Title)
       .IsRequired()
       .HasMaxLength(100);

builder.Property(engagement => engagement.StartDate)
       .IsRequired(false);

builder.Property(engagement => engagement.EndDate)
       .IsRequired(false);

builder.Property(engagement => engagement.Description)
       .IsRequired()
       .HasMaxLength(200);
        builder.Property(engagement => engagement.EngagementStatus).IsRequired().HasConversion<string>().HasMaxLength(50);
    }
}