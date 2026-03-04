namespace Nutrition_Tracker.Entities;

public class UserFoodLog
{
    public int Id {get;set;}
    
    public string ApplicationUserId { get; set; } // Foreign key to ApplicationUser
    public ApplicationUser ApplicationUser { get; set; }
    
    //Food item data
    public string FoodName { get; set; } 
    public string FdcId { get; set; } 
    public string GramsConsumed { get; set; } 
    
    // Nutrition Values Scaled to the amount consumed
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Carbohydrates { get; set; }
    public double Fat { get; set; }

    public DateTime LoggedTime { get; set; } = DateTime.UtcNow;
}