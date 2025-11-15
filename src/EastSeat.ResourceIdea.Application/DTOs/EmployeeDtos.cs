using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Application.DTOs;

/// <summary>
/// DTO for employee summary
/// </summary>
public record EmployeeDto(
    Guid Id,
    string StaffCode,
    string FullName,
    string Email,
    EmploymentStatus Status,
    Guid? RoleId,
    string? RoleName,
    bool IsSchedulable
);

/// <summary>
/// DTO for creating employees
/// </summary>
public record CreateEmployeeDto(
    string StaffCode,
    string FirstName,
    string LastName,
    string Email,
    DateTime HireDate,
    Guid? RoleId,
    decimal DefaultDailyCapacityHours = 8.0m
);
