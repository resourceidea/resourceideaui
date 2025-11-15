using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.Infrastructure.Identity;

/// <summary>
/// Custom application user extending Identity user
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public string DisplayName { get; set; } = string.Empty;
    public string? StaffCode { get; set; }
    public decimal DefaultDailyCapacityHours { get; set; } = 8.0m;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
}
