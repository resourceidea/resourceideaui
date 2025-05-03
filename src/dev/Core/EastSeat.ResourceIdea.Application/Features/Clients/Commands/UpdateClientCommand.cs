// ----------------------------------------------------------------------------------
// File: UpdateClientCommand.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Clients\Commands\UpdateClientCommand.cs
// Description: Command to update client.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Commands;

/// <summary>
/// Command to update client.
/// </summary>
public sealed class UpdateClientCommand : BaseRequest<ClientModel>
{
    /// <summary>DepartmentId of the client to update.</summary>
    public ClientId ClientId { get; set; }

    /// <summary>Client's name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Client address.</summary>
    public Address Address { get; set; } = Address.Empty;

    public Client ToEntity() => new()
    {
        Id = ClientId,
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