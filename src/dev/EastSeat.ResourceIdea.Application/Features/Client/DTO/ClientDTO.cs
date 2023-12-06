using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EastSeat.ResourceIdea.Application.Features.Client.DTO;

/// <summary>
/// Data transfer object that holds data used to create a client.
/// </summary>
public record ClientDTO
{
    /// <summary> Client ID. </summary>
    public Guid Id { get; set; }

    /// <summary> Client name. </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary> Client address. </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary> Client identification color code. </summary>
    public string ColorCode { get; set; } = string.Empty;

    /// <summary> Subscription ID. </summary>
    public Guid SubscriptionId { get; set; }
}
