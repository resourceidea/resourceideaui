// ===============================================================================================
// File: AddClientCommand.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Clients\Commands\AddClientCommand.cs
// Description: Command to add a new client.
// ===============================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Commands;

public sealed class AddClientCommand : BaseRequest<ClientModel>
{
    public string Name { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string Building { get; set; } = string.Empty; public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            Name.ValidateRequired(nameof(Name)),
            City.ValidateRequired(nameof(City)),
            Street.ValidateRequired(nameof(Street)),
            Building.ValidateRequired(nameof(Building)),
            TenantId.ValidateRequired()
        }
        .Where(message => !string.IsNullOrWhiteSpace(message)); ;

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }

    /// <summary>
    /// Converts the command to a Client entity.
    /// </summary>
    /// <returns>A new Client entity with properties from the command.</returns>
    public Client ToEntity() => new()
    {
        Id = ClientId.Create(Guid.NewGuid()),
        Name = Name,
        Address = Address.Create(Building, Street, City),
        TenantId = TenantId // Ensure tenant ID is assigned from the command
    };
}
