using EastSeat.ResourceIdea.Domain.Common;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Represents an organizational role (Partner, Manager, Staff, etc.)
/// </summary>
public class Role : AuditableEntity
{
    private Role() { } // EF Core

    public string Name { get; private set; } = string.Empty;
    public int SeniorityLevel { get; private set; }
    public decimal? DefaultHourlyRate { get; private set; }
    public string? Description { get; private set; }

    // Navigation properties
    public ICollection<Employee> Employees { get; private set; } = new List<Employee>();

    public static Result<Role> Create(string name, int seniorityLevel,
        decimal? defaultRate = null, string? description = null)
    {
        try
        {
            Guard.AgainstNullOrEmpty(name, nameof(name));
            Guard.AgainstOutOfRange(seniorityLevel, 1, 10, nameof(seniorityLevel));

            var role = new Role
            {
                Name = name.Trim(),
                SeniorityLevel = seniorityLevel,
                DefaultHourlyRate = defaultRate,
                Description = description?.Trim()
            };

            return Result<Role>.Success(role);
        }
        catch (Exception ex)
        {
            return Result<Role>.Failure(ex.Message);
        }
    }
}
