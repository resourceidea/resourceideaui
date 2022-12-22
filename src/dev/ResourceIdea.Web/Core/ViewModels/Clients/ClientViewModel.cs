namespace ResourceIdea.Core.ViewModels.Clients;

public record ClientViewModel(
    string? ClientId,
    string? Name, 
    string? Address, 
    string? Industry,
    bool? Active);