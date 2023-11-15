using AutoMapper;

using EastSeat.ResourceIdea.Application.Models;
using EastSeat.ResourceIdea.Persistence.Models;

namespace EastSeat.ResourceIdea.Persistence.MappingProfiles;

public class PersistenceMappingProfile : Profile
{
    public PersistenceMappingProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserViewModel>();
    }
}
