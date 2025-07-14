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
                     employeeId => employeeId == null ? null : employeeId.Value.ToString(),
                     value => string.IsNullOrEmpty(value) ? null : EmployeeId.Create(value));

              builder.Property(engagement => engagement.PartnerId)
                     .IsRequired(false)
                     .HasConversion(
                     employeeId => employeeId == null ? null : employeeId.Value.ToString(),
                     value => string.IsNullOrEmpty(value) ? null : EmployeeId.Create(value));

              builder.Property(engagement => engagement.Title)
                     .IsRequired(false)
                     .HasMaxLength(100);

              builder.Property(engagement => engagement.StartDate)
                     .IsRequired(false);

              builder.Property(engagement => engagement.EndDate)
                     .IsRequired(false);

              builder.Property(engagement => engagement.Description)
                     .IsRequired(false)
                     .HasMaxLength(200);

              builder.Property(engagement => engagement.EngagementStatus)
                     .IsRequired(true)
                     .HasConversion<string>()
                     .HasMaxLength(50);

               builder.Property(engagement => engagement.Color)
                     .IsRequired(false)
                     .HasMaxLength(10);

               builder.Property(engagement => engagement.MigrationProjectId)
                     .IsRequired(false)
                     .HasMaxLength(100);

               builder.Property(engagement => engagement.MigrationClientId)
                     .IsRequired(false)
                     .HasMaxLength(100);

               builder.Property(engagement => engagement.MigrationJobId)
                     .IsRequired(false)
                     .HasMaxLength(100);

               builder.Property(engagement => engagement.MigrationManager)
                     .IsRequired(false)
                     .HasMaxLength(40);

               builder.Property(engagement => engagement.MigrationPartner)
                     .IsRequired(false)
                     .HasMaxLength(40);
    }
}