using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using System.ComponentModel.DataAnnotations;

namespace EastSeat.ResourceIdea.Web.Controllers;

/// <summary>
/// Controller for handling authentication operations that require HTTP redirects.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Handles login with proper server-side redirect.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(
                request.Username,
                request.Password,
                request.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { success = true, redirectUrl = "/" });
            }
            else if (result.IsLockedOut)
            {
                return BadRequest(new { success = false, message = "Account is locked out. Please try again later." });
            }
            else if (result.IsNotAllowed)
            {
                return BadRequest(new { success = false, message = "Login not allowed. Please verify your account." });
            }
            else
            {
                return BadRequest(new { success = false, message = "Invalid username or password. Please try again." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login for user: {Username}", request.Username);
            return StatusCode(500, new { success = false, message = "An error occurred during login. Please try again." });
        }
    }

    /// <summary>
    /// Handles logout with proper server-side redirect.
    /// </summary>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return Ok(new { success = true, redirectUrl = "/" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during logout");
            return StatusCode(500, new { success = false, message = "An error occurred during logout. Please try again." });
        }
    }
}

/// <summary>
/// Request model for login operation.
/// </summary>
public class LoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
