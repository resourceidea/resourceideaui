namespace EastSeat.ResourceIdea.Domain.ValueObjects;

public sealed class ClientInput
{
    /// <summary>/// Client name. </summary>
    public string? Name { get; set; }

    /// <summary>/// Client address.</summary>
    public string? Address { get; set; }
}
