namespace EastSeat.ResourceIdea.Application.MappingProfiles;

using AutoMapper;

public sealed class ResourceIdeaMappingProfile : Profile
{
    public ResourceIdeaMappingProfile()
    {
        CreateMap<Domain.Entities.Tenant, Domain.Tenant.Models.TenantModel>();
    }
}