using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Common.Entities;

namespace EastSeat.ResourceIdea.Domain.Clients.Entities;

public class Client : BaseEntity
{
    public ClientId Id { get; set; }

    public Address Address { get; set; }

    public string? Name { get; set; }
}