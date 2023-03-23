namespace ResourceIdea.Pages.Engagements
{
    public class IndexModel : BasePageModel
    {
        private readonly IEngagementHandler _engagementHandler;
        private readonly IClientsHandler _clientsHandler;

        [FromQuery(Name = "client")]
        public string? Client { get; set; }

        public string? SubscriptionCode { get; set; }

        public ClientViewModel? EngagementClient { get; set; }

        public IList<EngagementViewModel>? Engagements { get; private set; }

        [BindProperty(SupportsGet = true)] public int CurrentPage { get; private set; } = 1;

        [BindProperty] public string? Search { get; set; }

        private decimal? Count { get; set; }

        private decimal? PageSize { get; set; } = 10;

        public int? TotalPages => (int)Math.Ceiling(decimal.Divide(Count ?? 0m, PageSize ?? 0m));
        
        public bool ShowPrevious => CurrentPage > 1;

        public bool ShowNext => CurrentPage < TotalPages;

        public bool ShowFirst => CurrentPage != 1;

        public bool ShowLast => CurrentPage != TotalPages;

        public IndexModel(IEngagementHandler engagementHandler, IClientsHandler clientsHandler)
        {
            _engagementHandler = engagementHandler;
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
            Engagements = await _engagementHandler.GetPaginatedResultAsync(SubscriptionCode, Client, CurrentPage, 10, search);
            EngagementClient = await _clientsHandler.GetClientByIdAsync(SubscriptionCode, Client);

            return Page();
        }

        public IActionResult OnPostSearch([FromForm] string client)
        {
            var page = CurrentPage;
            return RedirectToPage("Index", new { client, page, Search });
        }
    }
}
