namespace EastSeat.ResourceIdea.Infrastructure.Identity;

/// <summary>
/// Policy names for authorization
/// </summary>
public static class PolicyNames
{
    public const string ViewPlanner = "ViewPlanner";
    public const string ManageEngagements = "ManageEngagements";
    public const string ManageAssignments = "ManageAssignments";
    public const string ManageEmployees = "ManageEmployees";
    public const string ManageLeaveRequests = "ManageLeaveRequests";
    public const string ApproveLeaveRequests = "ApproveLeaveRequests";
    public const string ManagePublicHolidays = "ManagePublicHolidays";
    public const string ExecuteRollover = "ExecuteRollover";
    public const string AdminOnly = "AdminOnly";
}

/// <summary>
/// Role names
/// </summary>
public static class RoleNames
{
    public const string Admin = "Admin";
    public const string Partner = "Partner";
    public const string Manager = "Manager";
    public const string Staff = "Staff";
}
