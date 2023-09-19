using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EastSeat.ResourceIdea.Application.Responses;

/// <summary>
/// Response returned by API endpoints.
/// </summary>
public class ApiResponse<T> : BaseResponse where T : class
{
    public T Data { get; set; } = default!;

    /// <summary>
    /// Instantiates <see cref="ApiResponse{T}"/>
    /// </summary>
    /// <param name="data">Response data.</param>
    public ApiResponse(T data) : base()
    {
        Success = true;
        Data = data;
    }

    /// <summary>
    /// Instantiates <see cref="ApiResponse{T}"/>
    /// </summary>
    /// <param name="data">Response data.</param>
    /// <param name="message">Response message.</param>
    public ApiResponse(T data, string message) : base(message)
    {
        Success = true;
        Message = message;
        Data = data;
    }

    /// <summary>
    /// Instantiates <see cref="ApiResponse{T}"/>
    /// </summary>
    /// <param name="data">Response data.</param>
    /// <param name="success">True if the response is a success, otherwise False.</param>
    /// <param name="message">Response message.</param>
    /// <param name="errorCode">Response error code.</param>
    public ApiResponse(T data, bool success, string message, string errorCode) : base(success, message)
    {
        Success = success;
        Message = message;
        ErrorCode = errorCode;
        Data = data;
    }
}
