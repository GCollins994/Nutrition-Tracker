namespace Nutrition_Tracker.Models.Auth;

public class AuthResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string? Token { get; set; }
    public UserInfoDto? User { get; set; }
}

public class UserInfoDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public DateTime RegisteredAt { get; set; }
}