using ResourceIdeaUI.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ResourceIdeaUI.Pages.AuthPages
{
    public partial class Login
    {
        private Model model = new Model();
        private bool loading;
        private string error;

        protected override void OnInitialized()
        {
            // redirect to home if already logged in
            if (AuthenticationService.Token != null)
            {
                NavigationManager.NavigateTo("");
            }
        }

        private async void HandleValidSubmit()
        {
            loading = true;
            try
            {
                await AuthenticationService.Login(model.Username, model.Password);
                var returnUrl = NavigationManager.QueryString("returnUrl") ?? "/";
                NavigationManager.NavigateTo(returnUrl);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                loading = false;
                StateHasChanged();
            }
        }

        private class Model
        {
            [Required]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }
        }
    }
}
