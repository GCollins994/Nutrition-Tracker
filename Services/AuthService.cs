using Microsoft.AspNetCore.Identity;
using Nutrition_Tracker.Entities;
using Nutrition_Tracker.Models.Auth;

namespace Nutrition_Tracker.Services;


/// <summary>
/// Service for handling authentication-related operations, including Registration, Login, Token generation, etc.
/// </summary>
public class AuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user account.
    /// User input RegisterDto -> ApplicationUser entity.
    /// </summary>
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            _logger.LogInformation("Registration attempt for: {Email}", registerDto.Email);

            //Check if User Already Exists
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed: User already exists with email {Email}", registerDto.Email);
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "User with this email already exists."
                };
            }

            //Converting Dto to Entity    
            var user = new ApplicationUser
            {
                UserName = registerDto.Email, //Identity required UserName
                Email = registerDto.Email,
                EmailConfirmed = true //Skipping email confirmation for now
            };

            //Create User with Identity
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User regustered successfully: {Email}, {UserId}", registerDto.Email, user.Id);

                //Convers Entity back to Dto
                return new AuthResponseDto
                {
                    Success = true,
                    Message = "User created successfully",
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        RegisteredAt = DateTime.UtcNow // Set registration time to now
                    }
                };
            }
            else
            {
                //Identity validation failed
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("User registration failed: {Errors}", errors);

                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"User registration failed: {errors}"
                };
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception during user registration for email: {Email}", registerDto.Email);
            return new AuthResponseDto
            {
                Success = false,
                Message = "An error occurred during registration. Please try again later."
            };
        }
    }

    /// <summary>
    /// Login user with email and password.
    /// </summary>
    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        try
        {
            _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

            //Find user by email
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed, user not found: {Email}", loginDto.Email);
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            //Check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in successfuly: {Email}, {UserId}", loginDto.Email, user.Id);

                //Convert Entity to Dto
                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Login successful",
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        RegisteredAt = DateTime.UtcNow
                        //TODO: add registration date from user
                        //TODO: Add JWT Token generation
                    }
                };
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("Login failed - Account locked out: {Email}", loginDto.Email);
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Account is locked. Please try again later."
                };
            }
            else if (result.RequiresTwoFactor)
            {
                _logger.LogInformation("Login retuires two-factor autherntication: {Email}", loginDto.Email);
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Two-factor authentication is required."
                };
            }
            else
            {
                _logger.LogWarning("Login failed - Invalid credentials: {Email}", loginDto.Email);
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception during login for email: {Email}", loginDto.Email);
            return new AuthResponseDto
            {
                Success = false,
                Message = "An unexpected error occurred during login. Please try again later."
            };
        }
    }
}