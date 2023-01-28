using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ResourceIdea.Pages.Clients
{
    public class AddClientModel : BasePageModel
    {
        private readonly IClientsHandler _clientsHandler;

        public AddClientModel(IClientsHandler clientsHandler)
        {
            _clientsHandler = clientsHandler;
        }

        /// <summary>
        /// Address of the client being added.
        /// </summary>
        [BindProperty]
        public string? Address { get; set; }

        /// <summary>
        /// Name of the client being added.
        /// </summary>
        [BindProperty]
        public string? Name { get; set; }

        /// <summary>
        /// Client industry.
        /// </summary>
        [BindProperty]
        public string? Industry { get; set; }

        public void OnGet()
        {
        }

        public async Task<ActionResult> OnPost()
        {
            var subscriptionCode = GetSubscriptionCode();
            var newClientId = await _clientsHandler.AddAsync(subscriptionCode, new ClientViewModel
            (
                Guid.NewGuid().ToString(),
                Name,
                Address,
                Industry,
                true
            ));

            return RedirectToPage("details", new { id = newClientId });
        }
    }
}
