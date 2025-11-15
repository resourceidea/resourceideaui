using EastSeat.ResourceIdea.Domain.Common;
using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Represents an employee leave request
/// </summary>
public class LeaveRequest : AuditableEntity
{
    private LeaveRequest() { } // EF Core

    public Guid EmployeeId { get; private set; }
    public LeaveType LeaveType { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public LeaveRequestStatus Status { get; private set; }
    public string? Reason { get; private set; }
    public Guid? ApprovedBy { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    public bool IsPaid { get; private set; }
    public decimal WorkingDaysImpact { get; private set; }

    // Navigation properties
    public Employee Employee { get; private set; } = null!;
    public Employee? Approver { get; private set; }

    public static Result<LeaveRequest> Create(Guid employeeId, LeaveType leaveType,
        DateTime startDate, DateTime endDate, string? reason = null)
    {
        try
        {
            Guard.AgainstInvalidDateRange(startDate, endDate, nameof(startDate));

            var leaveRequest = new LeaveRequest
            {
                EmployeeId = employeeId,
                LeaveType = leaveType,
                StartDate = startDate.Date,
                EndDate = endDate.Date,
                Status = LeaveRequestStatus.Requested,
                Reason = reason?.Trim(),
                IsPaid = leaveType != LeaveType.Unpaid
            };

            return Result<LeaveRequest>.Success(leaveRequest);
        }
        catch (Exception ex)
        {
            return Result<LeaveRequest>.Failure(ex.Message);
        }
    }

    public void Approve(Guid approverId)
    {
        Status = LeaveRequestStatus.Approved;
        ApprovedBy = approverId;
        ApprovedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Reject(Guid approverId)
    {
        Status = LeaveRequestStatus.Rejected;
        ApprovedBy = approverId;
        ApprovedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = LeaveRequestStatus.Cancelled;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetWorkingDaysImpact(decimal days)
    {
        WorkingDaysImpact = days;
        ModifiedAt = DateTime.UtcNow;
    }
}
