using Microsoft.AspNetCore.Identity;

namespace EastSeat.ResourceIdea.DataStore.Identity.Entities
{
    /// <summary>
    /// Represents an application role claim.
    /// </summary>
    public class ApplicationIdentityRoleClaim : IdentityRoleClaim<string>
    {
        /// <summary>
        /// Gets or sets the role associated with the claim.
        /// </summary>
        public ApplicationRole? Role { get; set; }
    }
}