using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ResourceIdea.Pages.Clients;

public class ClientIndexModel : PageModel
{
    private readonly IClientsHandler _clientsHandler;
    private readonly ILogger<ClientIndexModel> _logger;

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; private set; } = 1;
    [BindProperty] public string? Search { get; set; }
    private decimal? Count { get; set; }
    private decimal? PageSize { get; set; } = 10;
    public int? TotalPages => (int) Math.Ceiling(decimal.Divide(Count ?? 0m, PageSize ?? 0m));
    [BindProperty(SupportsGet = true)] public string? SubscriptionCode { get; set; }
    public IList<ClientViewModel>? Clients { get; private set; }
    public bool ShowPrevious => CurrentPage > 1;
    public bool ShowNext => CurrentPage < TotalPages;
    public bool ShowFirst => CurrentPage != 1;
    public bool ShowLast => CurrentPage != TotalPages;

    public ClientIndexModel(IClientsHandler clientsHandler, ILogger<ClientIndexModel> logger)
    {
        _clientsHandler = clientsHandler;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet([FromQuery] int? page = 1, string? search = null)
    {
        CurrentPage = page ?? 1;
        Clients = await _clientsHandler.GetPaginatedResultAsync(SubscriptionCode, CurrentPage, 10, search);
        Count = await _clientsHandler.GetCountAsync(SubscriptionCode, search);

        return Page();
    }

    public IActionResult OnPostSearch()
    {
        var subscriptionCode = SubscriptionCode;
        var page = CurrentPage;
        return RedirectToPage("Index", new {subscriptionCode, page, Search});
    }
}