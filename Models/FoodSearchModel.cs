namespace Nutrition_Tracker.Models;

public class FoodSearchModel
{
    public int TotalHits { get; set; }
    public IEnumerable<Food> Foods { get; set; }
}

public class Food
{
    public string Description { get; set; }
    public string fdcId { get; set; }
}