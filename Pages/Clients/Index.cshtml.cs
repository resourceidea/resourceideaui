namespace ResourceIdea.Pages.Clients;

[Authorize]
public class ClientIndexModel : BasePageModel
{
    private readonly IClientsHandler _clientsHandler;

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; private set; } = 1;
    [BindProperty] public string? Search { get; set; }
    private decimal? Count { get; set; }
    private decimal? PageSize { get; set; } = 10;
    public int? TotalPages => (int) Math.Ceiling(decimal.Divide(Count ?? 0m, PageSize ?? 0m));
    private string? SubscriptionCode { get; set; }
    public IList<ClientViewModel>? Clients { get; private set; }
    public bool ShowPrevious => CurrentPage > 1;
    public bool ShowNext => CurrentPage < TotalPages;
    public bool ShowFirst => CurrentPage != 1;
    public bool ShowLast => CurrentPage != TotalPages;

    public ClientIndexModel(IClientsHandler clientsHandler)
    {
        _clientsHandler = clientsHandler;
    }

    public async Task<IActionResult> OnGet([FromQuery] int? page = 1, string? search = null)
    {
        CurrentPage = page ?? 1;

        var (isValidRequest, redirectLocation, subscriptionCode) = IsValidSubscriberRequest();
        if (!isValidRequest)
        {
            return redirectLocation;
        }

        SubscriptionCode = subscriptionCode;
        Clients = await _clientsHandler.GetPaginatedResultAsync(SubscriptionCode, CurrentPage, 10, search);
        Count = await _clientsHandler.GetCountAsync(SubscriptionCode, search);

        return Page();
    }

    public IActionResult OnPostSearch()
    {
        var page = CurrentPage;
        return RedirectToPage("Index", new {page, Search});
    }
}