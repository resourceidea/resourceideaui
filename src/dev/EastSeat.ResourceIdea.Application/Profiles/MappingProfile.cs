using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Asset.Commands;
using EastSeat.ResourceIdea.Application.Features.Asset.Queries.GetAssetsList;
using EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;
using EastSeat.ResourceIdea.Application.Features.Subscription.Queries.GetSubscriptionsList;
using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Application.Profiles;

/// <summary>
/// Mapping profile for AutoMapper.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Asset, CreateAssetDTO>();
        CreateMap<Asset, AssetListVM>();

        CreateMap<Subscription, CreateSubscriptionVM>();
        CreateMap<Subscription, SubscriptionsListVM>();
    }
}
