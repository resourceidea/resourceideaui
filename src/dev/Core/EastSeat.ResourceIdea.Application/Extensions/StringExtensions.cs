using EastSeat.ResourceIdea.Application.Exceptions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;

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
            .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim(), StringComparer.OrdinalIgnoreCase);
        
        ValidateFiltersDictionary(filterDictionary);

        return filterDictionary;
    }

    private static void ValidateFiltersDictionary(Dictionary<string, string> filterDictionary)
    {
        ThrowIfMalformedSubscriptionDateFilters(filterDictionary);
        ThrowIfMalformedSubscriptionPeriodFilters(filterDictionary);
    }

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
