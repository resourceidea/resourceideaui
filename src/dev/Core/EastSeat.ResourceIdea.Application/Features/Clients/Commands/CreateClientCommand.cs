// ----------------------------------------------------------------------------------
// File: CreateClientCommand.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Clients\Commands\CreateClientCommand.cs
// Description: Command to create a client.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Commands;

/// <summary>
/// Command to create a client.
/// </summary>
public class CreateClientCommand : BaseRequest<ClientModel>
{
    /// <summary>Client name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Client address</summary>
    public Address Address { get; set; } = Address.Empty;

    public Client ToEntity() => new()
    {
        Id = ClientId.Create(Guid.NewGuid()),
        Name = Name,
        Address = Address,
        TenantId = TenantId,
    };

    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            Name.ValidateRequired(nameof(Name)),
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}