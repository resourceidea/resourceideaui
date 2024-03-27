using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Persistence.Models;

namespace EastSeat.ResourceIdea.Persistence.Extensions;

internal static class ClientEntityExtensions
{
    internal static Client MapToModel(this ClientEntity entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address,
            ColorCode = entity.ColorCode
        };
}
