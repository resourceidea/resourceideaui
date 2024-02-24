using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Common;

/// <summary>
/// Base class for all paged list pages.
/// </summary>
public class PagedListSupportBase<T> : ComponentBase
{
    protected IReadOnlyList<T> Items = [];
    protected bool HasPreviousPage = false;
    protected bool HasNextPage = false;
    protected int CurrentPage = 1;
    protected string SearchTerm { get; set; } = string.Empty;
}

