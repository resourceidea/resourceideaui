using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.MappingProfiles;

public sealed class ResourceIdeaMappingProfile : Profile
{
    public ResourceIdeaMappingProfile()
    {        
        ConfigureTenantMappings();
        ConfigureSubscriptionServiceMappings();
        ConfigureSubscriptionMappings();
        ConfigureClientMappings();
        ConfigureEngagementMappings();
        ConfigureEngagementTaskMappings();
    }

    private void ConfigureTenantMappings()
    {
        CreateMap<Tenant, TenantModel>()
                    .ForMember(tenantModel => tenantModel.TenantId,
                               opt => opt.MapFrom(tenant => TenantId.Create(tenant.TenantId)));

        CreateMap<PagedListResponse<Tenant>, PagedListResponse<TenantModel>>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom((src, dest, destMember, context) => src.Items.Select(item => context.Mapper.Map<TenantModel>(item)).ToList()));

        CreateMap<ResourceIdeaResponse<Tenant>, ResourceIdeaResponse<TenantModel>>()
            .ForMember(
                dest => dest.Content,
                opt => opt.MapFrom((src, dest, destMember, context) => context.Mapper.Map<TenantModel>(src.Content)));

        CreateMap<ResourceIdeaResponse<PagedListResponse<Engagement>>, ResourceIdeaResponse<PagedListResponse<EngagementModel>>>()
            .ForMember(
                dest => dest.Content,
                opt => opt.MapFrom((src, dest, destMember, context) => context.Mapper.Map<PagedListResponse<EngagementModel>>(src.Content)));
    }

    private void ConfigureSubscriptionServiceMappings()
    {
        CreateMap<SubscriptionService, SubscriptionServiceModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => SubscriptionServiceId.Create(src.Id.Value)));

        CreateMap<ResourceIdeaResponse<SubscriptionService>, ResourceIdeaResponse<SubscriptionServiceModel>>()
            .ForMember(
                dest => dest.Content,
                opt => opt.MapFrom((src, dest, destMember, context) => context.Mapper.Map<SubscriptionServiceModel>(src.Content)));

        CreateMap<PagedListResponse<SubscriptionService>, PagedListResponse<SubscriptionServiceModel>>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom((src, dest, destMember, context) => src.Items.Select(item => context.Mapper.Map<SubscriptionServiceModel>(item)).ToList()));

        CreateMap<ResourceIdeaResponse<PagedListResponse<SubscriptionService>>, ResourceIdeaResponse<PagedListResponse<SubscriptionServiceModel>>>()
            .ForMember(
                dest => dest.Content,
                opt => opt.MapFrom((src, dest, destMember, context) => context.Mapper.Map<PagedListResponse<SubscriptionServiceModel>>(src.Content)));
    }

    private void ConfigureSubscriptionMappings()
    {
        CreateMap<Subscription, SubscriptionModel>()
                    .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => TenantId.Create(src.TenantId)))
                    .ForMember(dest => dest.SubscriptionServiceName, opt => opt.MapFrom(src => src.SubscriptionService != null
                                                                                             ? src.SubscriptionService.Name
                                                                                             : string.Empty));

        CreateMap<PagedListResponse<Subscription>, PagedListResponse<SubscriptionModel>>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom((src, dest, destMember, context) => src.Items.Select(item => context.Mapper.Map<SubscriptionModel>(item)).ToList()));

        CreateMap<PagedListResponse<Subscription>, PagedListResponse<SubscriptionModel>>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom((src, dest, destMember, context) => src.Items.Select(item => context.Mapper.Map<SubscriptionModel>(item)).ToList()));

        CreateMap<ResourceIdeaResponse<PagedListResponse<Subscription>>, ResourceIdeaResponse<PagedListResponse<SubscriptionModel>>>()
            .ForMember(
                dest => dest.Content,
                opt => opt.MapFrom((src, dest, destMember, context) => context.Mapper.Map<PagedListResponse<SubscriptionModel>>(src.Content)));
    }

    private void ConfigureClientMappings()
    {
        CreateMap<Client, ClientModel>()
                    .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => TenantId.Create(src.TenantId)))
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ClientId.Create(src.Id.Value)));

        CreateMap<ResourceIdeaResponse<Client>, ResourceIdeaResponse<ClientModel>>()
            .ForMember(
                dest => dest.Content,
                opt => opt.MapFrom((src, dest, destMember, context) => context.Mapper.Map<ClientModel>(src.Content)));

        CreateMap<PagedListResponse<Client>, PagedListResponse<ClientModel>>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom((src, dest, destMember, context) => src.Items.Select(item => context.Mapper.Map<ClientModel>(item)).ToList()));

        CreateMap<ResourceIdeaResponse<PagedListResponse<Client>>, ResourceIdeaResponse<PagedListResponse<ClientModel>>>()
            .ForMember(
                dest => dest.Content,
                opt => opt.MapFrom((src, dest, destMember, context) => context.Mapper.Map<PagedListResponse<ClientModel>>(src.Content)));
    }

    private void ConfigureEngagementMappings()
    {
        CreateMap<Engagement, EngagementModel>()
                    .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => TenantId.Create(src.TenantId)))
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => EngagementId.Create(src.Id.Value)));

        CreateMap<PagedListResponse<Engagement>, PagedListResponse<EngagementModel>>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom((src, dest, destMember, context) => src.Items.Select(item => context.Mapper.Map<EngagementModel>(item)).ToList()));

        CreateMap<ResourceIdeaResponse<Engagement>, ResourceIdeaResponse<EngagementModel>>()
            .ForMember(
                dest => dest.Content,
                opt => opt.MapFrom((src, dest, destMember, context) => context.Mapper.Map<EngagementModel>(src.Content)));

        CreateMap<ResourceIdeaResponse<PagedListResponse<Engagement>>, ResourceIdeaResponse<PagedListResponse<EngagementModel>>>()
            .ForMember(
                dest => dest.Content,
                opt => opt.MapFrom((src, dest, destMember, context) => context.Mapper.Map<PagedListResponse<EngagementModel>>(src.Content)));
    }

    private void ConfigureEngagementTaskMappings()
    {
        CreateMap<EngagementTask, EngagementTaskModel>()
                    .ForMember(dest => dest.EngagementId, opt => opt.MapFrom(src => EngagementId.Create(src.EngagementId.Value)))
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => EngagementId.Create(src.Id.Value)));

        CreateMap<PagedListResponse<Engagement>, PagedListResponse<EngagementModel>>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom((src, dest, destMember, context) => src.Items.Select(item => context.Mapper.Map<EngagementModel>(item)).ToList()));

        CreateMap<ResourceIdeaResponse<Engagement>, ResourceIdeaResponse<EngagementModel>>()
            .ForMember(
                dest => dest.Content,
                opt => opt.MapFrom((src, dest, destMember, context) => context.Mapper.Map<EngagementModel>(src.Content)));

        CreateMap<ResourceIdeaResponse<PagedListResponse<Engagement>>, ResourceIdeaResponse<PagedListResponse<EngagementModel>>>()
            .ForMember(
                dest => dest.Content,
                opt => opt.MapFrom((src, dest, destMember, context) => context.Mapper.Map<PagedListResponse<EngagementModel>>(src.Content)));
    }
}