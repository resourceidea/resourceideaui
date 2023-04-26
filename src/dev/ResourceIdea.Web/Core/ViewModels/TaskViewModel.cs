namespace ResourceIdea.Web.Core.ViewModels;

public record TaskViewModel
{
    public string? Id { get; set; }
    public string? ClientId { get; set; }
    public string? Client { get; set; }
    public string? EngagementId { get; set; }
    public string? Engagement { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Color { get; set; }
    public string? Manager { get; set; }
    public string? Partner { get; set; }
}
