namespace EastSeat.ResourceIdea.Domain.Exceptions;

/// <summary>
/// Invalid ItemId exception.
/// </summary>
public class ResourceIdeaException : Exception
{
    public ResourceIdeaException() { }

    public ResourceIdeaException(string message) : base(message) { }

    public ResourceIdeaException(string message, Exception innerException) : base(message, innerException) { }
}