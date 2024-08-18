namespace EastSeat.ResourceIdea.Application.Types;

/// <summary>
/// Represents the result of an operation that can either succeed or fail.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="Result{T}"/> class with the specified values.
/// </remarks>
/// <param name="value">The value of the result.</param>
/// <param name="isSuccess">A value indicating whether the result is a success.</param>
/// <param name="error">The error message if the result is a failure.</param>
public sealed class Result<T>(T? value, bool isSuccess, string error)
{
    /// <summary>
    /// Gets the value of the result.
    /// </summary>
    public T? Value { get; } = value;

    /// <summary>
    /// Gets a value indicating whether the result is a success.
    /// </summary>
    public bool IsSuccess { get; } = isSuccess;

    /// <summary>
    /// Gets a value indicating whether the result is a failure.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error message if the result is a failure.
    /// </summary>
    public string Error { get; } = error;

    /// <summary>
    /// Creates a new instance of the <see cref="Result{T}"/> class representing a successful result.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    /// <returns>A new instance of the <see cref="Result{T}"/> class representing a successful result.</returns>
    public static Result<T> Success(T value) => new(value, true, string.Empty);

    /// <summary>
    /// Creates a new instance of the <see cref="Result{T}"/> class representing a failed result.
    /// </summary>
    /// <param name="error">The error message.</param>
    /// <returns>A new instance of the <see cref="Result{T}"/> class representing a failed result.</returns>
    public static Result<T> Failure(string error) => new(default, false, error);
}