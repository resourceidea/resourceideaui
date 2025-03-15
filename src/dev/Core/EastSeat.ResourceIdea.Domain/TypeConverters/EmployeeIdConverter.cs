// ----------------------------------------------------------------------------------
// File: EmployeeIdConverter.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\TypeConverters\EmployeeIdConverter.cs
// Description: Employee ID type converter.
// ----------------------------------------------------------------------------------

using System.ComponentModel;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.TypeConverters;

/// <summary>
/// Converts EmployeeId to and from other representations.
/// </summary>
public class EmployeeIdConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
    {
        if (value is string stringValue)
        {
            return EmployeeId.Create(stringValue);
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is EmployeeId employeeId)
        {
            return employeeId.ToString();
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}