namespace ResourceIdea.Pages;

[AllowAnonymous]
public class SetupModel : PageModel
{
    public IActionResult OnGetAsync(string? returnUrl = null)
    {
        return Page();
    }
}