namespace EastSeat.ResourceIdea.Application.Types;

public sealed class ResourceIdeaResponse<T> where T : class
{
    /// <summary>True is the response is a success; Otherwise False.</summary>
    public bool Success { get; set; }

    /// <summary>Response message</summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>ErrorCode if the operation failed.</summary>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Content from a success response on an operation.</summary>
    public Optional<T> Content { get; set; }

    /// <summary>Initializes <see cref="ResourceIdeaResponse{T}"/>.</summary>
    public ResourceIdeaResponse()
    {
        Success = true;
    }

    /// <summary>
    /// Initializes <see cref="ResourceIdeaResponse{T}"/>.
    /// </summary>
    /// <param name="message">Response message.</param>
    public ResourceIdeaResponse(string message)
    {
        Success = true;
        Message = message;
    }

    /// <summary>
    /// Initializes <see cref="ResourceIdeaResponse{T}"/>.
    /// </summary>
    /// <param name="success">Response result. True if the response was a success; Otherwise, False.</param>
    /// <param name="message">Response message.</param>
    public ResourceIdeaResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    /// <summary>
    /// Initializes <see cref="ResourceIdeaResponse{T}"/>
    /// </summary>
    /// <param name="success">Response result. True if the response was a success; Otherwise, False.</param>
    /// <param name="message">Response message.</param>
    /// <param name="errorCode">ErrorCode on an unsuccessful response.</param>
    public ResourceIdeaResponse(bool success, string message, string errorCode)
    {
        Success = success;
        Message = message;
        ErrorCode = errorCode;
    }

    public static ResourceIdeaResponse<T> NotFound() => new()
    {
        Success = false,
        Message = "Resource not found.",
        ErrorCode = Enums.ErrorCode.ResourceNotFound.ToString()
    };
}