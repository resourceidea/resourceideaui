using EastSeat.ResourceIdea.Identity.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.Identity
{
    public class ResourceIdeaIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public ResourceIdeaIdentityDbContext()
        {
        }

        public ResourceIdeaIdentityDbContext(DbContextOptions<ResourceIdeaIdentityDbContext> options)
            : base(options)
        {
            
        }
    }
}
