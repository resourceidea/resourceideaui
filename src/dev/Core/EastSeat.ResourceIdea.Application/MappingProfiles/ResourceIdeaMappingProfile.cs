namespace EastSeat.ResourceIdea.Application.MappingProfiles;

using AutoMapper;

using EastSeat.ResourceIdea.Domain.Tenant.ValueObjects;

public sealed class ResourceIdeaMappingProfile : Profile
{
    public ResourceIdeaMappingProfile()
    {
        CreateMap<Domain.Entities.Tenant, Domain.Tenant.Models.TenantModel>()
            .ForMember(tenantModel => tenantModel.TenantId,
                       opt => opt.MapFrom(tenant => TenantId.Create(tenant.TenantId));
    }
}