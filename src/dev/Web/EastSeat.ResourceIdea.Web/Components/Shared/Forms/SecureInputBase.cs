using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace EastSeat.ResourceIdea.Web.Components.Shared.Forms;

/// <summary>
/// Enhanced input component with comprehensive validation and security features.
/// Follows Microsoft Blazor security best practices for input handling.
/// </summary>
/// <typeparam name="TValue">The type of the bound value</typeparam>
public class SecureInputBase<TValue> : InputBase<TValue>
{
    /// <summary>
    /// Maximum allowed length for input to prevent DoS attacks.
    /// </summary>
    [Parameter] public int MaxLength { get; set; } = 1000;

    /// <summary>
    /// Whether to trim whitespace from input.
    /// </summary>
    [Parameter] public bool TrimWhitespace { get; set; } = true;

    /// <summary>
    /// Whether to HTML encode the input value for XSS protection.
    /// </summary>
    [Parameter] public bool HtmlEncode { get; set; } = true;

    /// <summary>
    /// Custom validation patterns (regex) for additional security.
    /// </summary>
    [Parameter] public string? ValidationPattern { get; set; }

    /// <summary>
    /// Custom validation error message for pattern mismatch.
    /// </summary>
    [Parameter] public string PatternErrorMessage { get; set; } = "Input format is not valid.";

    /// <summary>
    /// List of forbidden strings to prevent injection attacks.
    /// </summary>
    [Parameter]
    public List<string> ForbiddenStrings { get; set; } = new()
    {
        "<script", "javascript:", "vbscript:", "onload=", "onerror=", "onclick=",
        "select ", "union ", "insert ", "update ", "delete ", "drop ", "exec ",
        "script>", "</script>", "eval(", "setTimeout(", "setInterval("
    };

    protected override bool TryParseValueFromString(string? value, out TValue result, out string validationErrorMessage)
    {
        result = default!;
        validationErrorMessage = string.Empty;

        // Handle null or empty values
        if (string.IsNullOrEmpty(value))
        {
            if (typeof(TValue) == typeof(string))
            {
                result = (TValue)(object)string.Empty;
                return true;
            }
            return false;
        }

        // Apply security validations
        var securityValidationResult = ValidateInputSecurity(value);
        if (!securityValidationResult.IsValid)
        {
            validationErrorMessage = securityValidationResult.ErrorMessage;
            return false;
        }

        // Process the value (trim, encode, etc.)
        var processedValue = ProcessInputValue(value);

        // Validate length
        if (processedValue.Length > MaxLength)
        {
            validationErrorMessage = $"Input cannot exceed {MaxLength} characters.";
            return false;
        }

        // Validate pattern if specified
        if (!string.IsNullOrEmpty(ValidationPattern))
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(processedValue, ValidationPattern))
            {
                validationErrorMessage = PatternErrorMessage;
                return false;
            }
        }

        // Try to convert to target type
        try
        {
            if (typeof(TValue) == typeof(string))
            {
                result = (TValue)(object)processedValue;
                return true;
            }
            else if (typeof(TValue) == typeof(int) || typeof(TValue) == typeof(int?))
            {
                if (int.TryParse(processedValue, out var intValue))
                {
                    result = (TValue)(object)intValue;
                    return true;
                }
            }
            else if (typeof(TValue) == typeof(decimal) || typeof(TValue) == typeof(decimal?))
            {
                if (decimal.TryParse(processedValue, out var decimalValue))
                {
                    result = (TValue)(object)decimalValue;
                    return true;
                }
            }
            else if (typeof(TValue) == typeof(DateTime) || typeof(TValue) == typeof(DateTime?))
            {
                if (DateTime.TryParse(processedValue, out var dateValue))
                {
                    result = (TValue)(object)dateValue;
                    return true;
                }
            }
            else if (typeof(TValue) == typeof(Guid) || typeof(TValue) == typeof(Guid?))
            {
                if (Guid.TryParse(processedValue, out var guidValue))
                {
                    result = (TValue)(object)guidValue;
                    return true;
                }
            }

            // Use default conversion for other types
            result = (TValue)Convert.ChangeType(processedValue, typeof(TValue));
            return true;
        }
        catch
        {
            validationErrorMessage = $"Unable to convert '{processedValue}' to {typeof(TValue).Name}.";
            return false;
        }
    }

    private (bool IsValid, string ErrorMessage) ValidateInputSecurity(string value)
    {
        // Check for forbidden strings that might indicate injection attempts
        foreach (var forbidden in ForbiddenStrings)
        {
            if (value.Contains(forbidden, StringComparison.OrdinalIgnoreCase))
            {
                return (false, "Input contains potentially unsafe content.");
            }
        }

        // Check for suspicious Unicode characters
        if (ContainsSuspiciousUnicode(value))
        {
            return (false, "Input contains invalid characters.");
        }

        return (true, string.Empty);
    }

    private string ProcessInputValue(string value)
    {
        // Trim whitespace if requested
        if (TrimWhitespace)
        {
            value = value.Trim();
        }

        // HTML encode if requested and dealing with string type
        if (HtmlEncode && typeof(TValue) == typeof(string))
        {
            value = System.Net.WebUtility.HtmlEncode(value);
        }

        return value;
    }

    private static bool ContainsSuspiciousUnicode(string input)
    {
        // Check for potentially dangerous Unicode characters
        var suspiciousRanges = new[]
        {
            // Various Unicode ranges that could be used for obfuscation
            (0x200B, 0x200F), // Zero-width characters
            (0x2028, 0x2029), // Line/paragraph separators
            (0xFEFF, 0xFEFF),  // Byte order mark
        };

        return input.Any(c => suspiciousRanges.Any(range => c >= range.Item1 && c <= range.Item2));
    }
}
