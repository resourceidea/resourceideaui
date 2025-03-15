// ----------------------------------------------------------------------------------
// File: JobPositionId.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\JobPositions\ValueObjects\JobPositionId.cs
// Description: JobPosition ID value object.
// ----------------------------------------------------------------------------------

using System.ComponentModel;
using EastSeat.ResourceIdea.Domain.TypeConverters;

namespace EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;

/// <summary>
/// JobPosition ID type.
/// </summary>
[TypeConverter(typeof(JobPositionIdConverter))]
public readonly record struct JobPositionId
{
    private JobPositionId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// JobPosition ID value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Create a new JobPosition ID from a Guid value.
    /// </summary>
    /// <param name="jobPositionId">Guid</param>
    /// <returns><see cref="JobPositionId"/></returns>
    /// <exception cref="ArgumentException">Thrown when Guid is empty.</exception>
    public static JobPositionId Create(Guid jobPositionId)
    {
        if (jobPositionId == Guid.Empty)
        {
            throw new ArgumentException(
                "Can not use empty Guid value to create JobPositionId.",
                nameof(jobPositionId));
        }

        return new JobPositionId(jobPositionId);
    }

    /// <summary>
    /// Create a new JobPosition ID from a string value.
    /// </summary>
    /// <param name="jobPositionId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Thrown when string value can not be parsed to Guid.</exception>
    public static JobPositionId Create(string jobPositionId)
    {
        if (!Guid.TryParse(jobPositionId, out var value))
        {
            throw new ArgumentException(
                "String value used to create JobPositionId is not a valid Guid.",
                nameof(jobPositionId));
        }

        return Create(value);
    }

    /// <summary>
    /// Convert JobPositionId to string.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Empty JobPositionId.
    /// </summary>
    public static JobPositionId Empty => new(Guid.Empty);

    /// <summary>
    /// Check if JobPositionId is empty.
    /// </summary>
    /// <returns>True if JobPositionId is empty, otherwise False.</returns>
    public bool IsEmpty() => this == JobPositionId.Empty;

    /// <summary>
    /// Validates that the job position ID is not empty.
    /// </summary>
    /// <returns>
    /// Empty string if the ID is valid, otherwise a validation error message.
    /// </returns>
    public string ValidateRequired()
    {
        return IsEmpty()
            ? "Job position ID is required."
            : string.Empty;
    }
}