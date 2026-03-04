using Microsoft.AspNetCore.Mvc;
using Nutrition_Tracker.Models.Auth;
using Nutrition_Tracker.Services;

namespace Nutrition_Tracker.Controllers;

/// <summary>
/// Authentication controller - Handles user registration and login.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(AuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user account
    /// </summary>
    /// <param name="registerDto">Email, Password, ConfirmPassword</param>
    /// <returns>AuthResponseDto with success status and message</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            //Input validation
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Registration attempt with invalid model state for email: {Email}", registerDto.Email ?? "Unknown");

                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid input data",
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            //Call registration service
            var result = await _authService.RegisterAsync(registerDto);

            //Return HTTP response
            if (result.Success)
            {
                _logger.LogInformation("User registration successful for email: {Email}", registerDto.Email);
                return Ok(result); // Return 200 OK with success message
            }
            else
            {
                _logger.LogWarning("User registration failed for email: {Email}. Reason: {Message}",
                    registerDto.Email, result.Message);
                return BadRequest(result);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occured during registration for email: {Email}",
                registerDto.Email ?? "Unknown");

                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = "An unexpected error occured, please try again later."
                });
        }
    }

    /// <summary>
    /// Login user with email and password.
    /// </summary>
    /// <param name="loginDto">User login credentials (email, password)</param>
    /// <returns>AuthResponseDto with user information if successful</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            //Validate input
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login attempt with invalid model state for email: {Email}",
                    loginDto?.Email ?? "Unknown");

                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid input data",
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            //Call login service
            var result = await _authService.LoginAsync(loginDto);
            //Return HTTP response
            if (result.Success)
            {
                _logger.LogInformation("User login successful for email: {Email}", loginDto.Email);
                return Ok(result); // Return 200 OK
            }
            else
            {
                _logger.LogWarning("User l,ogin failed for email: {Email}. Reason: {Message}", loginDto.Email,
                    result.Message);
                return Unauthorized(result); // Return 401 Unauthorized
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexperted error occured during login for email: {Email}", loginDto?.Email ?? "Unknown");

            return StatusCode(500, new AuthResponseDto
            {
                Success = false,
                Message = "An unexpected error occured, please try again later."
            }); // Return 500 Internal Server Error
        }
    }

    /// <summary>
    /// Get API information and available endpoints
    /// GET /api/auth/info
    /// </summary>
    /// <returns>API documentation</returns>
    [HttpGet("info")]
    public IActionResult GetApiInfo()
    {
        return Ok(new
        {
            Service = "Nutrition Tracker Authentication API",
            Version = "1.0",
            Endpoints = new
            {
                Register = new
                {
                    Method = "POST",
                    Url = "/api/auth/register",
                    Description = "Register a new user account",
                    RequiredFields = new[] { "email", "password", "confirmPassword" }
                },
                Login = new
                {
                    Method = "POST", 
                    Url = "/api/auth/login",
                    Description = "Login with email and password",
                    RequiredFields = new[] { "email", "password" }
                },
                Health = new
                {
                    Method = "GET",
                    Url = "/api/auth/health", 
                    Description = "Check if auth service is running"
                }
            },
            ExampleRegisterRequest = new
            {
                email = "user@example.com",
                password = "SecurePassword123!",
                confirmPassword = "SecurePassword123!"
            },
            ExampleLoginRequest = new
            {
                email = "user@example.com", 
                password = "SecurePassword123!"
            }
        });
    }
}