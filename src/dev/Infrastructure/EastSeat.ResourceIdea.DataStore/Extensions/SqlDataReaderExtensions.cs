// ===============================================================================================
// File: SqlDataReaderExtensions.cs
// Path: src/dev/Infrastructure/EastSeat.ResourceIdea.DataStore/Extensions/SqlDataReaderExtensions.cs
// Description: Extension methods for SqlDataReader to handle null-safe value retrieval.
// ===============================================================================================

using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using Microsoft.Data.SqlClient;

namespace EastSeat.ResourceIdea.DataStore.Extensions;

/// <summary>
/// Extension methods for SqlDataReader to handle null-safe value retrieval.
/// </summary>
public static class SqlDataReaderExtensions
{
    /// <summary>
    /// Gets a string value from the reader, returning null if the value is DBNull.
    /// </summary>
    /// <param name="reader">The SqlDataReader instance.</param>
    /// <param name="columnName">The name of the column to retrieve.</param>
    /// <returns>The string value or null if DBNull.</returns>
    public static string? GetNullableString(this SqlDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }

    /// <summary>
    /// Gets a string value from the reader, returning the default value if the value is DBNull.
    /// </summary>
    /// <param name="reader">The SqlDataReader instance.</param>
    /// <param name="columnName">The name of the column to retrieve.</param>
    /// <param name="defaultValue">The default value to return if DBNull.</param>
    /// <returns>The string value or the default value if DBNull.</returns>
    public static string GetStringOrDefault(this SqlDataReader reader, string columnName, string defaultValue = "")
    {
        return reader.GetNullableString(columnName) ?? defaultValue;
    }

    /// <summary>
    /// Gets an EmployeeId value from the reader, handling null values.
    /// </summary>
    /// <param name="reader">The SqlDataReader instance.</param>
    /// <param name="columnName">The name of the column to retrieve.</param>
    /// <returns>The EmployeeId value or EmployeeId.Empty if DBNull.</returns>
    public static EmployeeId GetEmployeeIdOrEmpty(this SqlDataReader reader, string columnName)
    {
        string? value = reader.GetNullableString(columnName);
        return value != null ? EmployeeId.Create(value) : EmployeeId.Empty;
    }

    /// <summary>
    /// Gets a JobPositionId value from the reader, handling null values.
    /// </summary>
    /// <param name="reader">The SqlDataReader instance.</param>
    /// <param name="columnName">The name of the column to retrieve.</param>
    /// <returns>The JobPositionId value or JobPositionId.Empty if DBNull.</returns>
    public static JobPositionId GetJobPositionIdOrEmpty(this SqlDataReader reader, string columnName)
    {
        string? value = reader.GetNullableString(columnName);
        return value != null ? JobPositionId.Create(value) : JobPositionId.Empty;
    }

    /// <summary>
    /// Gets a DepartmentId value from the reader, handling null values.
    /// </summary>
    /// <param name="reader">The SqlDataReader instance.</param>
    /// <param name="columnName">The name of the column to retrieve.</param>
    /// <returns>The DepartmentId value or DepartmentId.Empty if DBNull.</returns>
    public static DepartmentId GetDepartmentIdOrEmpty(this SqlDataReader reader, string columnName)
    {
        string? value = reader.GetNullableString(columnName);
        return value != null ? DepartmentId.Create(value) : DepartmentId.Empty;
    }

    /// <summary>
    /// Gets an ApplicationUserId value from the reader (required field).
    /// </summary>
    /// <param name="reader">The SqlDataReader instance.</param>
    /// <param name="columnName">The name of the column to retrieve.</param>
    /// <returns>The ApplicationUserId value.</returns>
    public static ApplicationUserId GetApplicationUserId(this SqlDataReader reader, string columnName)
    {
        string? value = reader.GetNullableString(columnName);
        return value != null ? ApplicationUserId.Create(value) : ApplicationUserId.Empty;
    }

    /// <summary>
    /// Gets a TenantId value from the reader (required field).
    /// </summary>
    /// <param name="reader">The SqlDataReader instance.</param>
    /// <param name="columnName">The name of the column to retrieve.</param>
    /// <returns>The TenantId value.</returns>
    public static TenantId GetTenantId(this SqlDataReader reader, string columnName)
    {
        return TenantId.Create(reader.GetString(reader.GetOrdinal(columnName)));
    }

    /// <summary>
    /// Gets an EmployeeId value from the reader (required field).
    /// </summary>
    /// <param name="reader">The SqlDataReader instance.</param>
    /// <param name="columnName">The name of the column to retrieve.</param>
    /// <returns>The EmployeeId value.</returns>
    public static EmployeeId GetEmployeeId(this SqlDataReader reader, string columnName)
    {
        return EmployeeId.Create(reader.GetString(reader.GetOrdinal(columnName)));
    }
}
