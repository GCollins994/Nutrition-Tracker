using Microsoft.AspNetCore.Identity;

namespace Nutrition_Tracker.Entities;

public class ApplicationUser : IdentityUser
{
    public UserProfile Profile { get; set; } // Navigation property to UserProfile, Specific user data
    
    public ICollection<UserFoodLog> FoodLogs { get; set; } //association with UserFoodLog, Logging food items for the user
}