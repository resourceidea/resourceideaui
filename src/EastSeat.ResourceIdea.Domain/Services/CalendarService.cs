namespace EastSeat.ResourceIdea.Domain.Services;

/// <summary>
/// Calendar service for working day calculations
/// </summary>
public static class CalendarService
{
    /// <summary>
    /// Checks if a date is a weekend (Saturday or Sunday)
    /// </summary>
    public static bool IsWeekend(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    /// <summary>
    /// Calculates working days between two dates (excluding weekends)
    /// </summary>
    public static int CalculateWorkingDays(DateTime startDate, DateTime endDate,
        IEnumerable<DateTime> publicHolidays)
    {
        if (startDate > endDate)
        {
            return 0;
        }

        var holidaySet = new HashSet<DateTime>(publicHolidays.Select(d => d.Date));
        var workingDays = 0;
        var current = startDate.Date;

        while (current <= endDate.Date)
        {
            if (!IsWeekend(current) && !holidaySet.Contains(current))
            {
                workingDays++;
            }
            current = current.AddDays(1);
        }

        return workingDays;
    }

    /// <summary>
    /// Gets all working days in a date range (excluding weekends and public holidays)
    /// </summary>
    public static IEnumerable<DateTime> GetWorkingDays(DateTime startDate, DateTime endDate,
        IEnumerable<DateTime> publicHolidays)
    {
        var holidaySet = new HashSet<DateTime>(publicHolidays.Select(d => d.Date));
        var current = startDate.Date;

        while (current <= endDate.Date)
        {
            if (!IsWeekend(current) && !holidaySet.Contains(current))
            {
                yield return current;
            }
            current = current.AddDays(1);
        }
    }

    /// <summary>
    /// Checks if a date is a working day (not weekend, not public holiday)
    /// </summary>
    public static bool IsWorkingDay(DateTime date, IEnumerable<DateTime> publicHolidays)
    {
        if (IsWeekend(date))
        {
            return false;
        }

        var holidaySet = new HashSet<DateTime>(publicHolidays.Select(d => d.Date));
        return !holidaySet.Contains(date.Date);
    }
}
