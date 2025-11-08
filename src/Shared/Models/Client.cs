using System.ComponentModel.DataAnnotations;

namespace EastSeat.ResourceIdea.Shared.Models;

public enum ClientStatus
{
    Active = 1,
    Inactive = 2
}

public sealed class Client
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Code { get; set; } = string.Empty;

    public ClientStatus Status { get; set; } = ClientStatus.Active;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}

public sealed class ClientCreateDto
{
    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Code { get; set; } = string.Empty;
}

public sealed class ClientUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Code { get; set; } = string.Empty;

    public ClientStatus Status { get; set; } = ClientStatus.Active;
}
