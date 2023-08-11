using ResourceIdea.Models;
using ResourceIdea.Pages;

namespace ResourceIdea.Web.Pages.Tasks
{
    public class AddTaskModel : BasePageModel
    {
        private readonly IEngagementService engagementService;
        private readonly ITaskService taskService;

        /// <summary>
        /// EngagementId where the new task belongs.
        /// </summary>
        [FromQuery(Name = "engagement")] public string? EngagementId { get; set; }

        /// <summary>
        /// EngagementId task to add.
        /// </summary>
        [BindProperty] public TaskViewModel? EngagementTask { get; set; }

        /// <summary>
        /// Instantiates <see cref="AddTaskModel"/>
        /// </summary>
        /// <param name="engagementService"></param>
        /// <param name="taskService"></param>
        public AddTaskModel(IEngagementService engagementService, ITaskService taskService)
        {
            this.engagementService = engagementService;
            this.taskService = taskService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var engagement = await GetEngagementAsync();
            EngagementTask = new TaskViewModel
            {
                EngagementId = engagement.EngagementId,
                Engagement = engagement.Name,
                ClientId = engagement.ClientId,
                Client = engagement.Client
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string subscriptionCode = GetSubscriptionCode();
            if (EngagementTask is not null)
            {
                EngagementTask.Status = "ACTIVE";
                EngagementTask.Color = $"#{GetRandomColor().ToUpper()}";
                await taskService.AddAsync(subscriptionCode, EngagementTask);
            }

            return RedirectToPage("index", new { engagement = EngagementTask?.EngagementId });
        }

        private async Task<EngagementViewModel> GetEngagementAsync()
        {
            if (EngagementId is null)
            {
                throw new ResourceIdeaException(HttpStatusCode.BadRequest, ErrorCode.BadRequest, "Engagement is required when adding a new engagement task.");
            }

            var engagement = await engagementService.GetEngagementByIdAsync(GetSubscriptionCode(), EngagementId);
            if (engagement is null)
            {
                throw new ResourceIdeaException(HttpStatusCode.NotFound, ErrorCode.NotFound, "Engagement not found.");
            }

            return engagement;
        }

        private string GetRandomColor()
        {
            int length = 6;
            char[] chars = { 'a', 'b', 'c', 'd', 'e', 'f', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            Random random = new();
            char[] randomChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                randomChars[i] = chars[index];
            }

            return new string(randomChars);
        }
    }
}