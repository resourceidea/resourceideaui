using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Application.DTOs;

/// <summary>
/// DTO for assignment
/// </summary>
public record AssignmentDto(
    Guid Id,
    Guid EngagementYearId,
    Guid EmployeeId,
    string EmployeeName,
    DateTime StartDate,
    DateTime EndDate,
    AllocationType AllocationType,
    int PercentAllocation,
    decimal? PlannedHours
);

/// <summary>
/// DTO for creating assignments
/// </summary>
public record CreateAssignmentDto(
    Guid EngagementYearId,
    Guid EmployeeId,
    DateTime StartDate,
    DateTime EndDate,
    AllocationType AllocationType,
    int PercentAllocation,
    decimal? PlannedHours
);

/// <summary>
/// DTO for resource planner year view
/// </summary>
public record PlannerYearDto(
    int Year,
    List<EmployeePlannerRowDto> Employees
);

/// <summary>
/// DTO for employee row in planner
/// </summary>
public record EmployeePlannerRowDto(
    Guid EmployeeId,
    string EmployeeName,
    string? RoleName,
    List<PlannerSegmentDto> Segments
);

/// <summary>
/// DTO for planner segment (assignment, leave, weekend, holiday)
/// </summary>
public record PlannerSegmentDto(
    DateTime StartDate,
    DateTime EndDate,
    string Type, // "Assignment", "Leave", "Weekend", "PublicHoliday"
    string? Label,
    string? Color,
    int? PercentAllocation
);
