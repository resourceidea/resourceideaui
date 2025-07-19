using EastSeat.ResourceIdea.Web.Components.Base;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Backend;

public partial class Dashboard : ResourceIdeaComponentBase
{
    private int totalUsers = 0;
    private int totalTenants = 0;
    private int activeEngagements = 0;
    private string systemHealth = "Good";
    private int avgResponseTime = 250;
    private int dailyRequests = 1247;
    private int memoryUsage = 68;
    private int cpuUsage = 23;

    private List<ActivityItem> recentActivities = new();

    protected override async Task OnInitializedAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Simulate loading dashboard data
            await Task.Delay(100); // Simulate API call

            // Sample data - in real implementation, this would come from services
            totalUsers = 156;
            totalTenants = 23;
            activeEngagements = 87;
            systemHealth = "Excellent";

            // Sample recent activities
            recentActivities = new List<ActivityItem>
            {
                new() { Timestamp = DateTime.Now.AddMinutes(-5), Event = "User Login", User = "john.doe@example.com", IsSuccess = true },
                new() { Timestamp = DateTime.Now.AddMinutes(-12), Event = "New Engagement Created", User = "jane.smith@example.com", IsSuccess = true },
                new() { Timestamp = DateTime.Now.AddMinutes(-18), Event = "Password Reset", User = "bob.wilson@example.com", IsSuccess = true },
                new() { Timestamp = DateTime.Now.AddMinutes(-25), Event = "Failed Login Attempt", User = "suspicious@example.com", IsSuccess = false },
                new() { Timestamp = DateTime.Now.AddMinutes(-31), Event = "Data Export", User = "admin@example.com", IsSuccess = true }
            };
        }, "Loading dashboard data");
    }

    private class ActivityItem
    {
        public DateTime Timestamp { get; set; }
        public string Event { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
    }
}