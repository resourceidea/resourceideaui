using System;
using System.ComponentModel;
using System.Globalization;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.TypeConverters;

/// <summary>
/// Converts ClientId to and from other representations.
/// </summary>
public class ClientIdConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string stringValue)
        {
            return base.ConvertFrom(context, culture, value);
        }

        return ClientId.TryCreate(stringValue, out var clientId)
            ? clientId
            : throw new NotSupportedException($"The value '{stringValue}' is not a valid {nameof(ClientId)}.");
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is ClientId clientId)
        {
            return clientId.ToString();
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
