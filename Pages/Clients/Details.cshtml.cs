namespace ResourceIdea.Pages.Clients;

public class ClientDetailsModel : PageModel
{
    private readonly IClientsHandler _clientsHandler;
    
    public ClientDetailsModel(IClientsHandler clientsHandler)
    {
        _clientsHandler = clientsHandler;
    }
    
    [BindProperty(SupportsGet = true)] public string? Id { get; set; }
    public ClientViewModel? Client { get; set; }
    public string? SubscriptionCode { get; set; }

    public async Task<IActionResult> OnGet()
    {
        SubscriptionCode = Request.Cookies["CompanyCode"];
        Client = await _clientsHandler.GetClientByIdAsync(SubscriptionCode, Id);

        return Page();
    }
}