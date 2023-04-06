namespace ResourceIdea.Web.Core.ViewModels;

public record TaskViewModel(
    string? TaskId,
    string? EngagementId,
    string? Description,
    string? Status,
    string? Color,
    string? Manager,
    string? Partner);
