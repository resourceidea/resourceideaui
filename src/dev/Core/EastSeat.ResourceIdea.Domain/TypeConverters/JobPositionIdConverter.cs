using System.ComponentModel;
using System.Globalization;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.TypeConverters;

/// <summary>
/// Converts a string to a <see cref="JobPositionId"/>.
/// </summary>
public class JobPositionIdConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        if (sourceType == typeof(string))
        {
            return true;
        }

        return base.CanConvertFrom(context, sourceType);
    }

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string stringValue && Guid.TryParse(stringValue, out Guid guidValue))
        {
            return JobPositionId.Create(guidValue);
        }

        return base.ConvertFrom(context, culture, value);
    }

    /// <inheritdoc />
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        if (destinationType == typeof(string))
        {
            return true;
        }

        return base.CanConvertTo(context, destinationType);
    }

    /// <inheritdoc />
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is JobPositionId jobPositionId)
        {
            return jobPositionId.Value.ToString();
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }
}