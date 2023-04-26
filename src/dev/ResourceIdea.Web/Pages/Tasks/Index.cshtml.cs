using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ResourceIdea.Pages;
using ResourceIdea.Web.Core.Handlers.Tasks;

namespace ResourceIdea.Web.Pages.Tasks
{
    public class TasksIndexModel : BasePageModel
    {
        private readonly ITaskHandler taskHandler;
        private readonly IEngagementHandler engagementHandler;

        [FromQuery(Name = "engagement")]
        public string? Engagement { get; set; }

        public string? SubscriptionCode { get; set; }

        public EngagementViewModel? TaskEngagement { get; set; }

        public IList<TaskViewModel>? Tasks { get; private set; }

        [BindProperty(SupportsGet = true)] public int CurrentPage { get; private set; } = 1;

        [BindProperty] public string? Search { get; set; }

        private decimal? Count { get; set; }

        private decimal? PageSize { get; set; } = 10;

        public int? TotalPages => (int)Math.Ceiling(decimal.Divide(Count ?? 0m, PageSize ?? 0m));

        public bool ShowPrevious => CurrentPage > 1;

        public bool ShowNext => CurrentPage < TotalPages;

        public bool ShowFirst => CurrentPage != 1;

        public bool ShowLast => CurrentPage != TotalPages;

        public TasksIndexModel(IEngagementHandler engagementHandler, ITaskHandler taskHandler)
        {
            this.engagementHandler = engagementHandler;
            this.taskHandler = taskHandler;
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
            Tasks = await taskHandler.GetPaginatedListByEngagementAsync(SubscriptionCode, Engagement, CurrentPage, pageSize:10, search:search);
            TaskEngagement = await engagementHandler.GetEngagementByIdAsync(SubscriptionCode, Engagement);

            return Page();
        }

        public IActionResult OnPostSearch([FromForm] string client)
        {
            var page = CurrentPage;
            return RedirectToPage("Index", new { client, page, Search });
        }
    }
}
