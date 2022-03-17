namespace ResourceIdea.Pages.Logout;

[AllowAnonymous]
public class LogoutIndexModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LogoutIndexModel(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    public IActionResult OnGet()
    {
        if (User.Identity is not {IsAuthenticated: true}) return RedirectToPage("/index");
        _signInManager.SignOutAsync();
        Response.Cookies.Delete("CompanyCode");
        return RedirectToPage("/index");
    }
}