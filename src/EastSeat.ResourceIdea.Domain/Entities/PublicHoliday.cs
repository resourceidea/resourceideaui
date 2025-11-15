using EastSeat.ResourceIdea.Domain.Common;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Represents a public holiday
/// </summary>
public class PublicHoliday : AuditableEntity
{
    private PublicHoliday() { } // EF Core

    public DateTime Date { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsRecurring { get; private set; }
    public string? Region { get; private set; }

    public static Result<PublicHoliday> Create(DateTime date, string name,
        bool isRecurring = false, string? region = null)
    {
        try
        {
            Guard.AgainstNullOrEmpty(name, nameof(name));

            var holiday = new PublicHoliday
            {
                Date = date.Date,
                Name = name.Trim(),
                IsRecurring = isRecurring,
                Region = region?.Trim()
            };

            return Result<PublicHoliday>.Success(holiday);
        }
        catch (Exception ex)
        {
            return Result<PublicHoliday>.Failure(ex.Message);
        }
    }
}
