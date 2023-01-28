using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ResourceIdea.Core.Handlers.Engagements;
using ResourceIdea.Core.ViewModels;

namespace ResourceIdea.Pages.Engagements
{
    public class AddEngagementModel : BasePageModel
    {
        private readonly IEngagementHandler engagementHandler;

        [FromQuery(Name = "client")]
        public string? Client { get; set; }

        /// <summary>
        /// Name of the engagement.
        /// </summary>
        [BindProperty]
        public string? Name { get; set; }

        /// <summary>
        /// ID of the client that owns the engagement.
        /// </summary>
        [BindProperty]
        public string? ClientId { get; set; }

        /// <summary>
        /// Instantiates <see cref="AddEngagementModel"/>
        /// </summary>
        /// <param name="engagementHandler"></param>
        public AddEngagementModel(IEngagementHandler engagementHandler)
        {
            this.engagementHandler = engagementHandler;
        }

        public void OnGet()
        {
        }

        public async Task<ActionResult> OnPost()
        {
            var subscriptionCode = GetSubscriptionCode();
            var engagementId = await engagementHandler.AddAsync(subscriptionCode, new EngagementViewModel
            (
                ProjectId: Guid.NewGuid().ToString(),
                Name: Name,
                ClientId: ClientId,
                Color: null  // We are not setting the color
            ));

            return RedirectToPage("index", new { client=ClientId });
        }
    }
}
