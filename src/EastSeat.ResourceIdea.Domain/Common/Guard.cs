namespace EastSeat.ResourceIdea.Domain.Common;

/// <summary>
/// Guard clauses for domain validation
/// </summary>
public static class Guard
{
    public static void AgainstNull<T>(T value, string parameterName) where T : class
    {
        if (value is null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }

    public static void AgainstNullOrEmpty(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or empty.", parameterName);
        }
    }

    public static void AgainstInvalidDateRange(DateTime start, DateTime end, string parameterName)
    {
        if (start > end)
        {
            throw new ArgumentException("Start date must be before or equal to end date.", parameterName);
        }
    }

    public static void AgainstOutOfRange(int value, int min, int max, string parameterName)
    {
        if (value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(parameterName,
                $"Value must be between {min} and {max}.");
        }
    }
}
