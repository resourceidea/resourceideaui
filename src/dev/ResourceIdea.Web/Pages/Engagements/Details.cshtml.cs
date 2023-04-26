namespace ResourceIdea.Pages.Engagements
{
    public class EngagementDetailsModel : BasePageModel
    {
        private readonly IEngagementHandler engagementHandler;

        /// <summary>
        /// Engagement ID from the route.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        /// <summary>
        /// Client ID from the query string.
        /// </summary>
        [FromQuery(Name = "client")]
        public string? Client { get; set; }

        /// <summary>
        /// Engagement to view.
        /// </summary>
        [BindProperty]
        public EngagementViewModel? Engagement { get; set; }

        public EngagementDetailsModel(IEngagementHandler engagementHandler)
        {
            this.engagementHandler = engagementHandler;
        }

        public async Task<IActionResult> OnGet()
        {
            var subscriptionCode = GetSubscriptionCode();
            Engagement = await engagementHandler.GetEngagementByIdAsync(subscriptionCode, Id);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            string subscriptionCode = GetSubscriptionCode();

            if (Engagement is not null)
            {
                await engagementHandler.UpdateAsync(subscriptionCode, Engagement);
            }

            return RedirectToPage(new { client = Engagement?.ClientId });
        }
    }
}
