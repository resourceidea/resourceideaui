namespace ResourceIdea.Pages;

public class BasePageModel : PageModel
{
    private string? TryGetCookie(string key)
    {
        return Request.Cookies[key];
    }

    protected (bool isValidRequest, IActionResult redirectLocation, string? subscriptionCode) IsValidSubscriberRequest()
    {
        var subscriptionCode = TryGetCookie("CompanyCode");
        var isValidRequest = false;
        IActionResult redirectLocation = Page();
        
        if (subscriptionCode is null)
        {
            redirectLocation = Redirect("~/logout");
        }
        else
        {
            isValidRequest = true;
        }

        return (isValidRequest, redirectLocation, subscriptionCode);
    }

    protected string? GetSubscriptionCode() => Request.Cookies["CompanyCode"];
}