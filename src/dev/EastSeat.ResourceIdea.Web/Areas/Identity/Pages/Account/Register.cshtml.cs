using System.ComponentModel.DataAnnotations;

using EastSeat.ResourceIdea.Application.Contracts.Identity;
using EastSeat.ResourceIdea.Application.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EastSeat.ResourceIdea.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IResourceIdeaAuthenticationService authenticationService;

        public RegisterModel(IResourceIdeaAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = default!;
        public string ReturnUrl { get; set; } = string.Empty;

        public void OnGet()
        {
            ReturnUrl = Url.Content("~/");
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            ReturnUrl = Url.Content("~/");
            if (ModelState.IsValid)
            {
                await authenticationService.RegisterUserAsync(new UserRegistrationRequest
                {
                    Email = Input.Email,
                    Password = Input.Password,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    SubscriptionId = new Guid("00000000-0000-0000-0000-000000000000")
                });
            }

            return Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [MinLength(1)]
            public string FirstName { get; set; } = string.Empty;

            [Required]
            [MinLength(1)]
            public string LastName { get; set; } = string.Empty;
        }
    }
}
