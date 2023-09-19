using EastSeat.ResourceIdea.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EastSeat.ResourceIdea.Persistence.Configurations;

/// <summary>
/// Assignment table configuration.
/// </summary>
public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.HasKey(assignment => assignment.Id);

        builder.ToTable("Assignment");
    }
}
