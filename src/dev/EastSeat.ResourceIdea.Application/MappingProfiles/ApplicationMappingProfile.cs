using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Assets.Commands;
using EastSeat.ResourceIdea.Application.Features.Assets.Queries.GetAssetsList;
using EastSeat.ResourceIdea.Application.Features.Clients.DTO;
using EastSeat.ResourceIdea.Application.Features.Employees.Commands.CreateEmployee;
using EastSeat.ResourceIdea.Application.Features.Engagements.DTO;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands.CreateSubscription;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Queries.GetSubscriptionsList;
using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Domain.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Profiles;

/// <summary>
/// Mapping profile for AutoMapper.
/// </summary>
public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<Asset, CreateAssetDTO>();
        CreateMap<Asset, AssetListVM>();

        CreateMap<Subscription, SubscriptionsListVM>();
        CreateMap<Subscription, CreateSubscriptionViewModel>();

        CreateMap<Employee, CreateEmployeeViewModel>();

        CreateMap<Client, ClientDTO>();
        CreateMap<Client, ClientListDTO>();

        CreateMap<ClientDTO, ClientInput>();

        CreateMap<Engagement, EngagementListDTO>();
    }
}
