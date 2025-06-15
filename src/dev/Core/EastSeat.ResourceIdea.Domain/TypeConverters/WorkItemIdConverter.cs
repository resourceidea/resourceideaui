// ----------------------------------------------------------------------------------
// File: WorkItemIdConverter.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\TypeConverters\WorkItemIdConverter.cs
// Description: Work Item ID type converter.
// ----------------------------------------------------------------------------------

using System.ComponentModel;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.TypeConverters;

/// <summary>
/// Converts WorkItemId to and from other representations.
/// </summary>
public class WorkItemIdConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
    {
        if (value is string stringValue)
        {
            return WorkItemId.Create(stringValue);
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is WorkItemId workItemId)
        {
            return workItemId.ToString();
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
