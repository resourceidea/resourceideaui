// ----------------------------------------------------------------------------------
// File: WorkItemId.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\WorkItems\ValueObjects\WorkItemId.cs
// Description: Work Item ID value object.
// ----------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using EastSeat.ResourceIdea.Domain.TypeConverters;

namespace EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;

/// <summary>
/// Work Item ID type.
/// </summary>
[TypeConverter(typeof(WorkItemIdConverter))]
public readonly record struct WorkItemId
{
    private WorkItemId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Work Item ID value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Create a new Work Item ID from a Guid value.
    /// </summary>
    /// <param name="workItemId">Guid</param>
    /// <returns><see cref="WorkItemId"/></returns>
    public static WorkItemId Create(Guid workItemId)
    {
        return new WorkItemId(workItemId);
    }

    /// <summary>
    /// Create a new Work Item ID from a string value.
    /// </summary>
    /// <param name="workItemId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Thrown when string value can not be parsed to Guid.</exception>
    public static WorkItemId Create(string workItemId)
    {
        if (!Guid.TryParse(workItemId, out var value))
        {
            throw new ArgumentException(
                "String value used to create WorkItemId is not a valid Guid.",
                nameof(workItemId));
        }

        return Create(value);
    }

    /// <summary>
    /// Creates a new WorkItemId with a randomly generated GUID.
    /// </summary>
    /// <returns>A new WorkItemId with a unique identifier.</returns>
    public static WorkItemId NewId()
    {
        return new WorkItemId(Guid.NewGuid());
    }

    /// <summary>
    /// Convert WorkItemId to string.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Empty WorkItemId.
    /// </summary>
    public static WorkItemId Empty => new(Guid.Empty);

    /// <summary>
    /// Check if WorkItemId is empty.
    /// </summary>
    /// <returns>True if WorkItemId is empty, otherwise False.</returns>
    public bool IsEmpty() => this == Empty;

    /// <summary>
    /// Validates that the work item ID is not empty.
    /// </summary>
    /// <returns>
    /// Empty string if the ID is valid, otherwise a validation error message.
    /// </returns>
    public string ValidateRequired()
    {
        return IsEmpty()
            ? "Work Item ID is required."
            : string.Empty;
    }
}
