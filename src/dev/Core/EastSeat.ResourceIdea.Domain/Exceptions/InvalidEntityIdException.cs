namespace EastSeat.ResourceIdea.Domain.Exceptions;

public class InvalidEntityIdException : ResourceIdeaException
{
    public InvalidEntityIdException() { }

    public InvalidEntityIdException(string message) : base(message) { }

    public InvalidEntityIdException(string message, Exception innerException) : base(message, innerException) { }
}
