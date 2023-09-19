using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EastSeat.ResourceIdea.Application.Models;

/// <summary>
/// Represents a request to register a user.
/// </summary>
public class UserRegistrationRequest
{
    /// <summary>First name.</summary>
    [Required]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Last names</summary>
    [Required]
    public string LastName { get; set; } = string.Empty;

    /// <summary>User's email</summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>User's password</summary>
    [Required]
    public string Password { get; set; } = string.Empty;

    /// <summary>Subscription Id.</summary>
    public Guid SubscriptionId { get; set; } = Guid.Empty;
}
