namespace ResourceIdea.Common.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> ApplyCustomFilters<T>(this IQueryable<T> query, Dictionary<string, string>? filters)
    {
        if (filters is not null)
        {
            foreach (var key in filters.Keys)
            {
                query = query.Where(item => item!.GetType().GetProperty(key)!.GetValue(item, null)!.Equals(filters[key]));
            }
        }
        return query;
    }
}
