using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid SubscriptionId { get; set; } = Guid.Empty;
}
