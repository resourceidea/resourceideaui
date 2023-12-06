namespace EastSeat.ResourceIdea.Application.Responses;

/// <summary>
/// Base response class.
/// </summary>
public class BaseResponse<T>
{
    /// <summary>
    /// True if response is a success, otherwise False.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Message associated with response.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Error code associated with the response.
    /// </summary>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>
    /// Errors on the response.
    /// </summary>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Initializes <see cref="BaseResponse"/>.
    /// </summary>
    public BaseResponse()
    {
        Success = true;
    }

    /// <summary>
    /// Initializes <see cref="BaseResponse"/>.
    /// </summary>
    /// <param name="message">Message associated with the response.</param>
    public BaseResponse(string message)
    {
        Success = true;
        Message = message;
    }

    /// <summary>
    /// Initializes <see cref="BaseResponse"/>.
    /// </summary>
    /// <param name="success">True if response is a success, otherwise False.</param>
    /// <param name="message">Message associated with the response.</param>
    public BaseResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    /// <summary>
    /// Instantiates <see cref="BaseResponse"/>
    /// </summary>
    /// <param name="success"></param>
    /// <param name="message"></param>
    /// <param name="errorCode"></param>
    public BaseResponse(bool success, string message, string errorCode)
    {
        Success = success;
        Message = message;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Response content.
    /// </summary>
    public T? Content { get; set; } = default;
}
