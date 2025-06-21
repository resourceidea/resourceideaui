// ----------------------------------------------------------------------------------
// File: EngagementIdConverter.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\TypeConverters\EngagementIdConverter.cs
// Description: Engagement ID type converter.
// ----------------------------------------------------------------------------------

using System.ComponentModel;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.TypeConverters;

/// <summary>
/// Converts EngagementId to and from other representations.
/// </summary>
public class EngagementIdConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
    {
        if (value is string stringValue)
        {
            return EngagementId.Create(stringValue);
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is EngagementId engagementId)
        {
            return engagementId.ToString();
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}