namespace ResourceIdea.Pages.Logout;

[AllowAnonymous]
public class LogoutIndexModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LogoutIndexModel(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    public void OnGet()
    {
        if (User.Identity is not {IsAuthenticated: true}) return;
        _signInManager.SignOutAsync();
        Response.Cookies.Delete("CompanyCode");
    }
}