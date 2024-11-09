using EastSeat.ResourceIdea.Domain.Common.Entities;

namespace EastSeat.ResourceIdea.Domain.Common.ValueObjects;

public readonly record struct Id<T> where T : BaseEntity
{
    public Guid Value { get; }

    public Id(Guid value)
    {
        Value = value;
    }

    public static Id<T> NewId() => new(Guid.NewGuid());

    public static Id<T> Empty => new(Guid.Empty);

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(Id<T> id) => id.Value;
    public static explicit operator Id<T>(Guid value) => new(value);
}
