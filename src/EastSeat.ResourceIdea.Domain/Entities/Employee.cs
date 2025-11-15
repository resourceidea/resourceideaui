using EastSeat.ResourceIdea.Domain.Common;
using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Represents an employee of the audit firm
/// </summary>
public class Employee : AuditableEntity
{
    private Employee() { } // EF Core

    public string StaffCode { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime HireDate { get; private set; }
    public DateTime? TerminationDate { get; private set; }
    public EmploymentStatus Status { get; private set; }
    public decimal DefaultDailyCapacityHours { get; private set; } = 8.0m;
    public Guid? RoleId { get; private set; }
    public bool IsSchedulable { get; private set; } = true;

    // Navigation properties
    public Role? Role { get; private set; }
    public ICollection<Assignment> Assignments { get; private set; } = new List<Assignment>();
    public ICollection<LeaveRequest> LeaveRequests { get; private set; } = new List<LeaveRequest>();

    public string FullName => $"{FirstName} {LastName}";

    public static Result<Employee> Create(string staffCode, string firstName, string lastName,
        string email, DateTime hireDate, Guid? roleId = null, decimal dailyCapacity = 8.0m)
    {
        try
        {
            Guard.AgainstNullOrEmpty(staffCode, nameof(staffCode));
            Guard.AgainstNullOrEmpty(firstName, nameof(firstName));
            Guard.AgainstNullOrEmpty(lastName, nameof(lastName));
            Guard.AgainstNullOrEmpty(email, nameof(email));
            Guard.AgainstOutOfRange((int)dailyCapacity, 1, 24, nameof(dailyCapacity));

            var employee = new Employee
            {
                StaffCode = staffCode.Trim().ToUpperInvariant(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                Email = email.Trim().ToLowerInvariant(),
                HireDate = hireDate.Date,
                Status = EmploymentStatus.Active,
                DefaultDailyCapacityHours = dailyCapacity,
                RoleId = roleId
            };

            return Result<Employee>.Success(employee);
        }
        catch (Exception ex)
        {
            return Result<Employee>.Failure(ex.Message);
        }
    }

    public void Terminate(DateTime terminationDate)
    {
        TerminationDate = terminationDate.Date;
        Status = EmploymentStatus.Terminated;
        IsSchedulable = false;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateRole(Guid roleId)
    {
        RoleId = roleId;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetSchedulable(bool isSchedulable)
    {
        IsSchedulable = isSchedulable;
        ModifiedAt = DateTime.UtcNow;
    }
}
