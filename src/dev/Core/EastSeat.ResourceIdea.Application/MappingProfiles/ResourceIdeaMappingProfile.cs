namespace EastSeat.ResourceIdea.Application.MappingProfiles;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.TenantManagement.Entities;
using EastSeat.ResourceIdea.Domain.TenantManagement.Models;
using EastSeat.ResourceIdea.Domain.TenantManagement.ValueObjects;

public sealed class ResourceIdeaMappingProfile : Profile
{
    private readonly IMapper _mapper;

    public ResourceIdeaMappingProfile(IMapper mapper)
    {
        _mapper = mapper;

        CreateMap<Tenant, TenantModel>()
            .ForMember(tenantModel => tenantModel.TenantId,
                       opt => opt.MapFrom(tenant => TenantId.Create(tenant.TenantId)));

        CreateMap<PagedList<Tenant>, PagedList<TenantModel>>()
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(
                src => src.Items.Select(item => _mapper.Map<TenantModel>(item)).ToList()));
    }
}