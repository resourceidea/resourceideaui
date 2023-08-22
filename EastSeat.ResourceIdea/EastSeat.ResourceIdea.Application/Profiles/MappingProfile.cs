using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Asset.Commands;
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
    }
}
