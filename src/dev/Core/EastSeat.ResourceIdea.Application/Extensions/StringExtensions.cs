using EastSeat.ResourceIdea.Application.Exceptions;

namespace EastSeat.ResourceIdea.Application.Extensions;

/// <summary>
/// Extensions for the <see cref="string"/> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Gets the filters as a dictionary.
    /// </summary>
    /// <param name="input">Filters as a string value.</param>
    /// <param name="delimiter">Used to split the input into parts of filters.</param>
    /// <param name="keyValueSeparator">Splits the filter parts into key value pairs.</param>
    /// <returns>Filters as a dictionary of key value pairs.</returns>
    public static Dictionary<string, string> GetFiltersAsDictionary(this string input, char[] delimiter, char[] keyValueSeparator)
    {
        if (string.IsNullOrEmpty(input))
        {
            return [];
        }

        var filterDictionary = input
            .Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
            .Select(part => part.Split(keyValueSeparator, StringSplitOptions.RemoveEmptyEntries))
            .Where(parts => parts.Length == 2)
            .ToDictionary(parts => parts[0].Trim().ToLower(), parts => parts[1].Trim().ToLower(), StringComparer.OrdinalIgnoreCase);

        ValidateFiltersDictionary(filterDictionary);

        return filterDictionary;
    }

    /// <summary>
    /// Throws an exception if the string argument is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="argument">The string argument to validate.</param>
    /// <returns>Value being checked</returns>
    /// <exception cref="ArgumentException">Thrown when the argument is null, empty, or white-space.</exception>
    public static string ThrowIfNullOrEmptyOrWhiteSpace(this string? argument)
    {
        ArgumentException.ThrowIfNullOrEmpty(argument);
        ArgumentException.ThrowIfNullOrWhiteSpace(argument);

        return argument;
    }

    /// <summary>
    /// Validates the filters dictionary for any malformed subscription date or period filters.
    /// </summary>
    /// <param name="filterDictionary">The dictionary of filters to validate.</param>
    /// <exception cref="MalformedQueryFilterException">Thrown when the filters are malformed.</exception>
    private static void ValidateFiltersDictionary(Dictionary<string, string> filterDictionary)
    {
        ThrowIfMalformedSubscriptionDateFilters(filterDictionary);
        ThrowIfMalformedSubscriptionPeriodFilters(filterDictionary);
    }

    /// <summary>
    /// Throws an exception if the subscription period filters are malformed.
    /// </summary>
    /// <param name="filterDictionary">The dictionary of filters to validate.</param>
    /// <exception cref="MalformedQueryFilterException">Thrown when the subscription period filters are malformed.</exception>
    private static void ThrowIfMalformedSubscriptionPeriodFilters(Dictionary<string, string> filterDictionary)
    {
        bool hasSubscribedBeforeDateFilter = filterDictionary.ContainsKey("subbefore");
        bool hasSubscribedAfterDateFilter = filterDictionary.ContainsKey("subafter");

        if (hasSubscribedBeforeDateFilter && hasSubscribedAfterDateFilter)
        {
            return;
        }

        throw new MalformedQueryFilterException("Must have 'subbefore' and 'subafter' filters in the same query.");
    }

    /// <summary>
    /// Throws an exception if the subscription date filters are malformed.
    /// </summary>
    /// <param name="filterDictionary">The dictionary of filters to validate.</param>
    /// <exception cref="MalformedQueryFilterException">Thrown when the subscription date filters are malformed.</exception>
    private static void ThrowIfMalformedSubscriptionDateFilters(Dictionary<string, string> filterDictionary)
    {
        bool hasSubscribedOnDateFilter = filterDictionary.ContainsKey("subon");
        bool hasSubscribedBeforeDateFilter = filterDictionary.ContainsKey("subbefore");
        bool hasSubscribedAfterDateFilter = filterDictionary.ContainsKey("subafter");

        if (hasSubscribedOnDateFilter && (hasSubscribedBeforeDateFilter || hasSubscribedAfterDateFilter))
        {
            throw new MalformedQueryFilterException("Cannot have 'subon' filter with 'subbefore' or 'subafter' filters in the same query.");
        }
    }
}
