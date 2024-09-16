using EastSeat.ResourceIdea.Domain.Exceptions;

namespace EastSeat.ResourceIdea.Domain.Departments.ValueObjects;

/// <summary>
/// Represents the ID of a department.
/// </summary>
public readonly record struct DepartmentId
{
    /// <summary> The value of the department ID. </summary>
    public Guid Value { get; }

    private DepartmentId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="DepartmentId"/> class.
    /// </summary>
    /// <param name="value">Department Id as a string.</param>
    /// <returns>Instance of <see cref="DepartmentId"/>.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when creating a new <see cref="DepartmentId"/> from an empty Guid.</exception>
    public static DepartmentId Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidEntityIdException("DepartmentId cannot be empty");
        }

        if (!Guid.TryParse(value, out var departmentId))
        {
            throw new InvalidEntityIdException("DepartmentId is not a valid Guid");
        }

        if (departmentId.Equals(Guid.Empty))
        {
            throw new InvalidEntityIdException("DepartmentId cannot be an empty Guid");
        }

        return new DepartmentId(departmentId);
    }

    /// <summary>Creates an empty instance of the <see cref="DepartmentId"/> class.</summary>
    public static DepartmentId Empty => new(Guid.Empty);
}
