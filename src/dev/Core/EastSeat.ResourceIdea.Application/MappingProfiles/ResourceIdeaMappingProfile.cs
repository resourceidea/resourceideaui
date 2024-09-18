using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.ApplicationUsers.Commands;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.Models;

namespace EastSeat.ResourceIdea.Application.MappingProfiles;

public sealed class ResourceIdeaMappingProfile : Profile
{
    private readonly IMapper _mapper;

    public ResourceIdeaMappingProfile(IMapper mapper)
    {
        _mapper = mapper;

        CreateMap<Tenant, TenantModel>()
            .ForMember(tenantModel => tenantModel.TenantId,
                       opt => opt.MapFrom(tenant => TenantId.Create(tenant.TenantId)));

        CreateMap<PagedListResponse<Tenant>, PagedListResponse<TenantModel>>()
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(
                src => src.Items.Select(item => _mapper.Map<TenantModel>(item)).ToList()));

        CreateMap<SubscriptionService, SubscriptionServiceModel>();

        CreateMap<Subscription, SubscriptionModel>()
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => TenantId.Create(src.TenantId)))
            .ForMember(dest => dest.SubscriptionServiceName, opt => opt.MapFrom(src => src.SubscriptionService != null
                                                                                     ? src.SubscriptionService.Name
                                                                                     : string.Empty));

        CreateMap<PagedListResponse<Subscription>, PagedListResponse<SubscriptionModel>>()
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(
                src => src.Items.Select(item => _mapper.Map<SubscriptionModel>(item)).ToList()));
        
        CreateMap<Client, ClientModel>()
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => TenantId.Create(src.TenantId)))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ClientId.Create(src.Id.Value)));

        CreateMap<LoginModel, LoginCommand>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
    }
}