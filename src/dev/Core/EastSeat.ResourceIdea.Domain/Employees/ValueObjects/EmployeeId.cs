// ----------------------------------------------------------------------------------
// File: EmployeeId.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\Employees\ValueObjects\EmployeeId.cs
// Description: Employee ID value object.
// ----------------------------------------------------------------------------------

using System.ComponentModel;
using EastSeat.ResourceIdea.Domain.TypeConverters;

namespace EastSeat.ResourceIdea.Domain.Employees.ValueObjects;

/// <summary>
/// Employee ID type.
/// </summary>
[TypeConverter(typeof(EmployeeIdConverter))]
public readonly record struct EmployeeId
{
    private EmployeeId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Employee ID value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Create a new Employee ID from a Guid value.
    /// </summary>
    /// <param name="employeeId">Guid</param>
    /// <returns><see cref="EmployeeId"/></returns>
    /// <exception cref="ArgumentException">Thrown when Guid is empty.</exception>
    public static EmployeeId Create(Guid employeeId)
    {
        // Validation is commented out because the ManagerId can be empty in some cases.
        // TODO: Create a new type of ManagerId should be created with out the empty check.
        //if (employeeId == Guid.Empty)
        //{
        //    throw new ArgumentException(
        //        "Can not use empty Guid value to create EmployeeId.",
        //        nameof(employeeId));
        //}

        return new EmployeeId(employeeId);
    }

    /// <summary>
    /// Create a new Employee ID from a string value.
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Thrown when string value can not be parsed to Guid.</exception>
    public static EmployeeId Create(string employeeId)
    {
        if (!Guid.TryParse(employeeId, out var value))
        {
            throw new ArgumentException(
                "String value used to create EmployeeId is not a valid Guid.",
                nameof(employeeId));
        }

        return Create(value);
    }

    /// <summary>
    /// Creates a new EmployeeId with a randomly generated GUID.
    /// </summary>
    /// <returns>A new EmployeeId with a unique identifier.</returns>
    public static EmployeeId NewId()
    {
        return new EmployeeId(Guid.NewGuid());
    }

    /// <summary>
    /// Convert EmployeeId to string.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Empty EmployeeId.
    /// </summary>
    public static EmployeeId Empty => new(Guid.Empty);

    /// <summary>
    /// Check if EmployeeId is empty.
    /// </summary>
    /// <returns>True if EmployeeId is empty, otherwise False.</returns>
    public bool IsEmpty() => this == Empty;

    /// <summary>
    /// Validates that the employee ID is not empty.
    /// </summary>
    /// <returns>
    /// Empty string if the ID is valid, otherwise a validation error message.
    /// </returns>
    public string ValidateRequired()
    {
        return IsEmpty()
            ? "Employee ID is required."
            : string.Empty;
    }
}