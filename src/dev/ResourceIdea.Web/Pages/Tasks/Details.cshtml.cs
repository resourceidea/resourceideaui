using ResourceIdea.Pages;

namespace ResourceIdea.Web.Pages.Tasks
{
    public class TaskDetailsModel : BasePageModel
    {
        private readonly ITaskHandler taskHandler;

        public TaskDetailsModel(ITaskHandler taskHandler)
        {
            this.taskHandler = taskHandler;
        }

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        [BindProperty]
        public TaskViewModel? EngagementTask { get; set; }

        public async Task<IActionResult> OnGet()
        {
            string subscriptionCode = GetSubscriptionCode();
            EngagementTask = await taskHandler.GetTaskByIdAsync(subscriptionCode, Id);

            return Page();
        }
    }
}
