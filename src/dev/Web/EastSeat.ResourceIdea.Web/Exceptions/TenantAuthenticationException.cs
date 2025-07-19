using System;

namespace EastSeat.ResourceIdea.Web.Exceptions;

/// <summary>
/// Exception thrown when tenant information is missing or invalid, requiring re-authentication.
/// </summary>
public class TenantAuthenticationException : Exception
{
    public TenantAuthenticationException() : base()
    {
    }

    public TenantAuthenticationException(string message) : base(message)
    {
    }

    public TenantAuthenticationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
