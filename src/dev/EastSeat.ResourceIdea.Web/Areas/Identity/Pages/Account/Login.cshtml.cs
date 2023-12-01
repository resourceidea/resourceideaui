using System.ComponentModel.DataAnnotations;

using EastSeat.ResourceIdea.Application.Contracts.Identity;
using EastSeat.ResourceIdea.Application.Models;
using EastSeat.ResourceIdea.Domain.Enums;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EastSeat.ResourceIdea.Web.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IResourceIdeaAuthenticationService authenticationService;

        public LoginModel(IResourceIdeaAuthenticationService authenticationService)
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
                var response = await authenticationService.AuthenticateUserAsync(new AuthenticationRequest
                {
                    Email = Input.Email,
                    Password = Input.Password
                }, ResourceIdeaAuthenticationOption.Web);

                if (response.Success)
                {
                    return LocalRedirect(ReturnUrl);
                }
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
        }
    }
}
