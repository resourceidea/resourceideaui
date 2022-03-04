using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ResourceIdea.Areas.Clients.Pages;

// [Authorize]
public class IndexModel : PageModel
{
    private readonly IClientsHandler _clientsHandler;
    private readonly ILogger<IndexModel> _logger;

    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;
    public decimal? Count { get; private set; }
    public decimal? PageSize { get; private set; } = 10;

    public int? TotalPages => (int) Math.Ceiling(decimal.Divide(Count ?? 0m, PageSize ?? 0m));

    public string? SubscriptionCode { get; private set; }
    public IList<ClientViewModel>? Clients { get; private set; }

    public bool ShowPrevious => CurrentPage > 1;
    public bool ShowNext => CurrentPage < TotalPages;
    public bool ShowFirst => CurrentPage != 1;
    public bool ShowLast => CurrentPage != TotalPages;

    public IndexModel(IClientsHandler clientsHandler, ILogger<IndexModel> logger)
    {
        _clientsHandler = clientsHandler;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet([FromRoute] string? subscriptionCode, [FromQuery] int? page = 1, string? search = null)
    {
        SubscriptionCode = subscriptionCode;
        CurrentPage = page ?? 1;
        Clients = await _clientsHandler.GetPaginatedResultAsync(SubscriptionCode, CurrentPage, 10, search);
        Count = await _clientsHandler.GetCountAsync(SubscriptionCode, search);

        return Page();
    }
    
    public IActionResult OnPostSearch(string? search)
    {
        var subscriptionCode = SubscriptionCode;
        var page = CurrentPage;
        return RedirectToPage("/Clients/Index", new {subscriptionCode, page, search});
    }
}