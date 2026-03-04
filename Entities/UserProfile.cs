namespace Nutrition_Tracker.Entities;

public class UserProfile
{
    public int Id {get;set;}
    
    public string ApplicationUserId { get; set; } // Foreign key to ApplicationUser
    public ApplicationUser ApplicationUser { get; set; } // Navigation property to ApplicationUser
    
    public double HeightCm { get; set; } // Height in centimeters
    public double HeightInches { get; set; } // Height in inches
    public double WeightKg { get; set; } // Weight in kilograms
    public double WeightLbs { get; set; } // Weight in pounds
    public int Age { get; set; } // Age in years
    public string Gender { get; set; } //M, F, Other
}