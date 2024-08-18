using EastSeat.ResourceIdea.Application.Enums;

namespace EastSeat.ResourceIdea.Application.Types;

/// <summary>
/// Represents the response of an operation that can either succeed or fail.
/// </summary>
/// <typeparam name="T">The type of the content.</typeparam>
/// <remarks>
/// Initializes <see cref="ResourceIdeaResponse{T}"/>
/// </remarks>
/// <param name="value">Content from a successful response on an operation.</param>
/// <param name="isSuccess">Response result. True if the response was a success; Otherwise, False.</param>
/// <param name="errorCode">ErrorCode on an unsuccessful response.</param>
public class ResourceIdeaResponse<T> where T : class
{
    /// <summary>Content from a success response on an operation.</summary>
    public Optional<T> Content { get; set; }

    /// <summary>True is the response is a success; Otherwise False.</summary>
    public bool IsSuccess { get; set; }

    /// <summary>True if the response is a failure; Otherwise False.</summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>ErrorCode if the operation failed.</summary>
    public ErrorCode Error { get; set; }

    private ResourceIdeaResponse(Optional<T> value, bool isSuccess, ErrorCode error)
    {
        Content = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ResourceIdeaResponse{T}"/> class representing a successful response.
    /// </summary>
    /// <param name="value">Content from a successful response on an operation.</param>
    /// <returns>A new instance of the <see cref="ResourceIdeaResponse{T}"/> class representing a successful response.</returns>
    public static ResourceIdeaResponse<T> Success(Optional<T> value) => new(value, true, ErrorCode.None);

    /// <summary>
    /// Creates a new instance of the <see cref="ResourceIdeaResponse{T}"/> class representing a failed response.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <returns>A new instance of the <see cref="ResourceIdeaResponse{T}"/> class representing a failed response.</returns>
    public static ResourceIdeaResponse<T> Failure(ErrorCode errorCode) => new(Optional<T>.None, false, errorCode);

    /// <summary>
    /// Creates a new instance of the <see cref="ResourceIdeaResponse{T}"/> class representing a failed response.
    /// </summary>
    public static ResourceIdeaResponse<T> NotFound() => Failure(ErrorCode.NotFound);

    /// <summary>
    /// Creates a new instance of the <see cref="ResourceIdeaResponse{T}"/> class representing a failed response.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onSuccess"></param>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public TResult Match<TResult>(Func<Optional<T>, TResult> onSuccess, Func<ErrorCode, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Content) : onFailure(Error);
    }
}