namespace ResourceIdea.Pages;

[Authorize]
public class IndexModel : PageModel
{
    // create readonly logger property
    private readonly ILogger<IndexModel> logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        this.logger = logger;
    }
    public void OnGet()
    {
        logger.LogInformation("Loading resource planner landing page.");
    }
}