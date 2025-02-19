// -----------------------------------------------------------------------
// File: StringExtensions.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\Extensions\StringExtensions.cs
// Description: String extensions.
// -----------------------------------------------------------------------

namespace EastSeat.ResourceIdea.Domain.Extensions;

public static class StringExtensions
{
    public static string ValidateRequired(this string value, string propertyName)
    {
        return string.IsNullOrEmpty(value)
            ? $"{propertyName} is required."
            : string.Empty;
    }
}