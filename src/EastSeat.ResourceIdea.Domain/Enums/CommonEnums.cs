namespace EastSeat.ResourceIdea.Domain.Enums;

public enum EngagementType
{
    Audit = 1,
    TaxCompliance = 2,
    TaxAdvisory = 3,
    Assurance = 4,
    Consulting = 5,
    Training = 6,
    Other = 99
}

public enum EngagementStatus
{
    Planned = 1,
    Active = 2,
    Suspended = 3,
    Completed = 4,
    Cancelled = 5
}

public enum EmploymentStatus
{
    Active = 1,
    OnLeave = 2,
    Terminated = 3,
    Suspended = 4
}

public enum LeaveType
{
    PaidTimeOff = 1,
    Sick = 2,
    Maternity = 3,
    Paternity = 4,
    Study = 5,
    Compassionate = 6,
    Unpaid = 7,
    Other = 99
}

public enum LeaveRequestStatus
{
    Requested = 1,
    Approved = 2,
    Rejected = 3,
    Cancelled = 4
}

public enum AllocationType
{
    FullTime = 1,
    PartTime = 2,
    Surge = 3
}
