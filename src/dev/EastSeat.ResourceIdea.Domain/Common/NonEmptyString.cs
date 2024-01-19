namespace EastSeat.ResourceIdea.Domain.Common;

public record struct NonEmptyString(string Value)
{
    public static implicit operator string(NonEmptyString value) => value.Value;

    public static implicit operator NonEmptyString(string value)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
        }

        return new NonEmptyString(value);
    }

    public override readonly string ToString() => Value;
}
