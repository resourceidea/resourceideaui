namespace EastSeat.ResourceIdea.Domain.Exceptions;

/// <summary>
/// Exception thrown when the tenant to update is not found.
/// </summary>
public class UpdateItemNotFoundException : ResourceIdeaException
{
    public UpdateItemNotFoundException() { }

    public UpdateItemNotFoundException(string message) : base(message) { }

    public UpdateItemNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
