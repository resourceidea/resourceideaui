namespace EastSeat.ResourceIdea.Application.DTOs;

/// <summary>
/// DTO for client summary
/// </summary>
public record ClientDto(
    Guid Id,
    string Name,
    string? RegistrationNumber,
    string? Sector,
    bool IsActive
);

/// <summary>
/// DTO for creating/updating clients
/// </summary>
public record CreateClientDto(
    string Name,
    string? RegistrationNumber,
    string? Sector,
    string? ContactEmail,
    string? ContactPhone
);
