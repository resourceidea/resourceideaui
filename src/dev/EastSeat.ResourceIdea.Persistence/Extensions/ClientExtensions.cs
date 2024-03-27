using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Persistence.Models;

namespace EastSeat.ResourceIdea.Persistence.Extensions;

internal static class ClientExtensions
{
    internal static ClientEntity MapToEntity(this Client client) =>
        new()
        {
            Id = client.Id,
            Name = client.Name,
            Address = client.Address,
            ColorCode = client.ColorCode
        };
}
