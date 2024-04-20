using EastSeat.ResourceIdea.Domain.Exceptions;

namespace EastSeat.ResourceIdea.Application.Exceptions;

/// <summary>
/// Thrown when a supplied query filter is malformed or wrongly written.
/// </summary>
public sealed class MalformedQueryFilterException : ResourceIdeaException
{
    public MalformedQueryFilterException() { }

    public MalformedQueryFilterException(string message) : base(message) { }

    public MalformedQueryFilterException(string message, Exception innerException) : base(message, innerException) { }
}
