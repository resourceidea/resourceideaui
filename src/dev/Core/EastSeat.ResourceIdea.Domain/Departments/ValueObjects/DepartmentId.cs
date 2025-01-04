﻿using EastSeat.ResourceIdea.Domain.Exceptions;

namespace EastSeat.ResourceIdea.Domain.Departments.ValueObjects;

/// <summary>
/// Department ID type.
/// </summary>
public readonly record struct DepartmentId
{
    /// <summary>
    /// Department ID value.
    /// </summary>
    public Guid Value { get; }

    private DepartmentId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Create a new Department ID from a Guid value.
    /// </summary>
    /// <param name="value">Guid</param>
    /// <returns>Department ID.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when the Guid is empty.</exception>
    public static DepartmentId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityIdException("DepartmentId cannot be empty");
        }

        return new DepartmentId(value);
    }

    /// <summary>
    /// Create a new Department ID from a string value.
    /// </summary>
    /// <param name="value">String representation of a department ID.</param>
    /// <returns>Department ID.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when string value can not be parsed to Guid.</exception>
    public static DepartmentId Create(string value)
    {
        if (!Guid.TryParse(value, out var departmentId))
        {
            throw new InvalidEntityIdException("DepartmentId is not a valid Guid");
        }

        return Create(departmentId);
    }
}