using EastSeat.ResourceIdea.Web.Components.Base;

namespace EastSeat.ResourceIdea.Web.Components.Pages.Backend;

public partial class SystemHealth : ResourceIdeaComponentBase
{
    private string overallStatus = "Healthy";
    private List<SystemAlert> systemAlerts = new();
    private List<ServiceDependency> serviceDependencies = new();

    protected override async Task OnInitializedAsync()
    {
        await ExecuteAsync(async () =>
        {
            await LoadSystemHealth();
        }, "Loading system health data");
    }

    private async Task LoadSystemHealth()
    {
        // Simulate loading health data
        await Task.Delay(200);

        overallStatus = "Healthy";

        // Sample alerts - normally would come from monitoring system
        systemAlerts = new List<SystemAlert>
        {
            new() { Level = "Warning", Message = "Memory usage above 70%", Timestamp = DateTime.Now.AddMinutes(-15) }
        };

        // Sample service dependencies
        serviceDependencies = new List<ServiceDependency>
        {
            new() { Name = "Database Server", IsHealthy = true, ResponseTime = 15, LastCheck = DateTime.Now.AddSeconds(-30) },
            new() { Name = "Azure Active Directory", IsHealthy = true, ResponseTime = 45, LastCheck = DateTime.Now.AddSeconds(-25) },
            new() { Name = "File Storage", IsHealthy = true, ResponseTime = 8, LastCheck = DateTime.Now.AddSeconds(-20) },
            new() { Name = "Email Service", IsHealthy = true, ResponseTime = 120, LastCheck = DateTime.Now.AddSeconds(-35) },
            new() { Name = "Backup Service", IsHealthy = false, ResponseTime = 0, LastCheck = DateTime.Now.AddMinutes(-5) }
        };
    }

    private class SystemAlert
    {
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    private class ServiceDependency
    {
        public string Name { get; set; } = string.Empty;
        public bool IsHealthy { get; set; }
        public int ResponseTime { get; set; }
        public DateTime LastCheck { get; set; }
    }
}