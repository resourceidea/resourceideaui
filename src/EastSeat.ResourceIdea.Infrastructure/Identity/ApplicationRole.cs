using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.Infrastructure.Identity;

/// <summary>
/// Custom application role extending Identity role
/// </summary>
public class ApplicationRole : IdentityRole<Guid>
{
    public int SeniorityLevel { get; set; }
    public decimal? DefaultHourlyRate { get; set; }
    public string? Description { get; set; }
}
