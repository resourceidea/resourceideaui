namespace EastSeat.ResourceIdea.Domain.Types;

/// <summary>
/// Represents an abstract class for the Either type, which can hold either a value of type TLeft or a value of type TRight.
/// </summary>
/// <typeparam name="TLeft">The type of the left value.</typeparam>
/// <typeparam name="TRight">The type of the right value.</typeparam>
public abstract class Either<TLeft, TRight>
{
    /// <summary>
    /// Maps the left value of the Either to a new value of type TNewLeft using the provided mapping function.
    /// </summary>
    /// <typeparam name="TNewLeft">The type of the new left value.</typeparam>
    /// <param name="mapping">The mapping function.</param>
    /// <returns>A new instance of Either with the left value mapped to type TNewLeft.</returns>
    public abstract Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> mapping);

    /// <summary>
    /// Maps the right value of the Either to a new value of type TNewRight using the provided mapping function.
    /// </summary>
    /// <typeparam name="TNewRight">The type of the new right value.</typeparam>
    /// <param name="mapping">The mapping function.</param>
    /// <returns>A new instance of Either with the right value mapped to type TNewRight.</returns>
    public abstract Either<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> mapping);

    /// <summary>
    /// Reduces the Either to a value of type TLeft using the provided mapping function if the Either holds a right value.
    /// </summary>
    /// <param name="mapping">The mapping function.</param>
    /// <returns>The reduced value of type TLeft.</returns>
    public abstract TLeft Reduce(Func<TRight, TLeft> mapping);

    /// <summary>
    /// Matches the Either to a result of type TResult using the provided functions for left and right values.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="onFailure">The function to handle the left value.</param>
    /// <param name="onSuccess">The function to handle the right value.</param>
    /// <returns>The result of type TResult.</returns>
    public abstract TResult Match<TResult>(Func<TLeft, TResult> onFailure, Func<TRight, TResult> onSuccess);
}

/// <summary>
/// Represents a success response of the Either type, which holds a right value.
/// </summary>
/// <typeparam name="TLeft">The type of the left value.</typeparam>
/// <typeparam name="TRight">The type of the right value.</typeparam>
/// <remarks>
/// Initializes a new instance of the SuccessResponse class with the specified right value.
/// </remarks>
/// <param name="value">The right value.</param>
public class SuccessResponse<TLeft, TRight>(TRight value) : Either<TLeft, TRight>
{
    private TRight Value { get; } = value;

    /// <inheritdoc />
    public override Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> mapping)
        => new SuccessResponse<TNewLeft, TRight>(Value);

    /// <inheritdoc />
    public override Either<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> mapping)
        => new SuccessResponse<TLeft, TNewRight>(mapping(Value));

    /// <inheritdoc />
    public override TLeft Reduce(Func<TRight, TLeft> mapping) => mapping(Value);

    /// <inheritdoc />
    public override TResult Match<TResult>(Func<TLeft, TResult> onFailure, Func<TRight, TResult> onSuccess)
        => onSuccess(Value);
}

/// <summary>
/// Represents a failure response of the Either type, which holds a left value.
/// </summary>
/// <typeparam name="TLeft">The type of the left value.</typeparam>
/// <typeparam name="TRight">The type of the right value.</typeparam>
/// <remarks>
/// Initializes a new instance of the FailureResponse class with the specified left value.
/// </remarks>
/// <param name="value">The left value.</param>
public class FailureResponse<TLeft, TRight>(TLeft value) : Either<TLeft, TRight>
{
    private TLeft Value { get; } = value;

    /// <inheritdoc />
    public override Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> mapping)
        => new FailureResponse<TNewLeft, TRight>(mapping(Value));

    /// <inheritdoc />
    public override Either<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> mapping)
        => new FailureResponse<TLeft, TNewRight>(Value);

    /// <inheritdoc />
    public override TLeft Reduce(Func<TRight, TLeft> mapping) => Value;

    /// <inheritdoc />
    public override TResult Match<TResult>(Func<TLeft, TResult> onFailure, Func<TRight, TResult> onSuccess)
        => onFailure(Value);
}