namespace EastSeat.ResourceIdea.Web.Components.Common;

public class ListHandler<T>
{
    public int CurrentPage { get; private set; } = 1;
    public bool SortAscending { get; private set; } = true;
    public string SortIcon { get; private set; } = "fa-sort";

    public async Task LoadNextPage(Func<int, Task> loadItems)
    {
        CurrentPage += 1;
        await loadItems(CurrentPage);
    }

    public async Task LoadPreviousPage(Func<int, Task> loadItems)
    {
        CurrentPage = CurrentPage > 1 ? CurrentPage - 1 : 1;
        await loadItems(CurrentPage);
    }

    public void SortItems(ref IReadOnlyList<T> items, Func<T, object> keySelector)
    {
        if (SortAscending)
        {
            items = [.. items.OrderBy(keySelector)];
            SortAscending = false;
            SortIcon = "fa-sort-up";
        }
        else
        {
            items = [.. items.OrderByDescending(keySelector)];
            SortAscending = true;
            SortIcon = "fa-sort-down";
        }
    }

    public async Task SearchItems(Func<int, Task> searchItems)
    {
        await searchItems(CurrentPage);
    }
}
