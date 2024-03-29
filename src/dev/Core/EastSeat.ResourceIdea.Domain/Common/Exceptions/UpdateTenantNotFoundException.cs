namespace EastSeat.ResourceIdea.Domain.Common.Exceptions;

/// <summary>
/// Exception thrown when the tenant to update is not found.
/// </summary>
public class UpdateTenantNotFoundException : ResourceIdeaException
{
    public UpdateTenantNotFoundException() { }

    public UpdateTenantNotFoundException(string message) : base(message) { }

    public UpdateTenantNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
