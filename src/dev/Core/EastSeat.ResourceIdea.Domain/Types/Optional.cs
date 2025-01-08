namespace EastSeat.ResourceIdea.Domain.Types;

public readonly struct Optional<T> //where T : BaseModel<T>
{
    private readonly T _value;
    public T Value
    {
        get
        {
            if (HasValue)
            {
                return _value;
            }
            throw new InvalidOperationException("Option has no value");
        }
    }

    public bool HasValue { get; }

    private Optional(T value)
    {
        _value = value;
        HasValue = true;
    }

    public static Optional<T> Some(T value) => new(value);

    public static Optional<T> None => new();

    public static implicit operator Optional<T>(T value) => value is null ? None : Some(value);

    public static explicit operator T(Optional<T> optional) => optional.Value;

    /// <summary>
    /// Performs an action on the value whether it exists or not.
    /// </summary>
    /// <param name="some">Action to perform if value exists.</param>
    /// <param name="none">Action to perform if no value exists.</param>
    /// <returns>Instance of <see cref="{Tresult}"/>.</returns>
    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none) => HasValue ? some(_value) : none();

    // /// <summary>
    // /// Returns the value if it exists, otherwise returns the default value.
    // /// </summary>
    // /// <param name="defaultValue">Default value.</param>
    // /// <returns>Instance of <see cref="T"/>.</returns>
    // public T GetValueOrDefault(T defaultValue) => HasValue && _value != null ? _value : defaultValue;

    // /// <summary>
    // /// Performs an action on the value if it exists.
    // /// </summary>
    // /// <param name="action">Action to perform.</param>
    // /// <returns>Instance of <see cref="{TResult}"/>.</returns>
    // public TResult IfHasValue<TResult>(Func<T, TResult> action, TResult defaultValue) =>
    //     HasValue ? action(_value) : defaultValue;

    // /// <summary>
    // /// Performs an action if the value does not exist.
    // /// </summary>
    // /// <param name="action">Action to perform.</param>
    // /// <param name="defaultValue">Default value.</param>
    // /// <returns>Instance of <see cref="{TResult}"/>.</returns>
    // public TResult IfNone<TResult>(Func<TResult> action, TResult defaultValue) =>
    //     HasValue is false ? action() : defaultValue;
}