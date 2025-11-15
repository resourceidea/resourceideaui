namespace EastSeat.ResourceIdea.Application.Common;

/// <summary>
/// Interface for accessing the current authenticated user
/// </summary>
public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
}
