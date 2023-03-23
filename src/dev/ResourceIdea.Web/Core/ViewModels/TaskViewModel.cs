namespace ResourceIdea.Web.Core.ViewModels;

public record TaskViewModel(
    string? JobId,
    string? ProjectId,
    string? Description,
    string? Status,
    string? Color,
    string? Manager,
    string? Partner);
