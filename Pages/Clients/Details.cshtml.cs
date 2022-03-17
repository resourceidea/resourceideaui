namespace ResourceIdea.Pages.Clients;

public class ClientDetailsModel : PageModel
{
    private readonly IClientsHandler _clientsHandler;
    
    public ClientDetailsModel(IClientsHandler clientsHandler)
    {
        _clientsHandler = clientsHandler;
    }
    
    [BindProperty(SupportsGet = true)] public Guid? Id { get; set; }

    public void OnGet()
    {
    }
}