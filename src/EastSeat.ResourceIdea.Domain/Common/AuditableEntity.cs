namespace EastSeat.ResourceIdea.Domain.Common;

/// <summary>
/// Base entity with common audit properties
/// </summary>
public abstract class AuditableEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public Guid? CreatedBy { get; init; }
    public DateTime? ModifiedAt { get; set; }
    public Guid? ModifiedBy { get; set; }
}
