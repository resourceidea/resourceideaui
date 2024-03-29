using EastSeat.ResourceIdea.Domain.Common.Exceptions;

namespace EastSeat.ResourceIdea.Domain.Tenant.ValueObjects;

public record struct TenantId
{
    public Guid Value { get; }

    private TenantId(Guid value)
    {
        Value = value;
    }

    public static TenantId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityIdException("TenantId cannot be empty");
        }

        return new TenantId(value);
    }
}