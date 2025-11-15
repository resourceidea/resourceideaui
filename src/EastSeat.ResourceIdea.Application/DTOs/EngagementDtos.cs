using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Application.DTOs;

/// <summary>
/// DTO for engagement summary
/// </summary>
public record EngagementDto(
    Guid Id,
    Guid ClientId,
    string ClientName,
    string Code,
    string Title,
    EngagementType Type,
    EngagementStatus Status,
    DateTime StartDate,
    DateTime? EndDate,
    Guid? PartnerId,
    Guid? ManagerId,
    string? PartnerName,
    string? ManagerName
);

/// <summary>
/// DTO for creating engagements
/// </summary>
public record CreateEngagementDto(
    Guid ClientId,
    string Code,
    string Title,
    EngagementType Type,
    DateTime StartDate,
    DateTime? EndDate,
    Guid? PartnerId,
    Guid? ManagerId,
    decimal? BudgetHours
);
