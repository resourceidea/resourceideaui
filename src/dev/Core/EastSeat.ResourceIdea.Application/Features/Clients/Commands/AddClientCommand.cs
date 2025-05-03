using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Commands;

public sealed class AddClientCommand
{
    public string Name { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string Building { get; set; } = string.Empty;
}
