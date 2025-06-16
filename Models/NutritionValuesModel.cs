namespace Nutrition_Tracker.Models;

/// <summary>
/// Model for Nutrition Values of a food item from the Food Data Central API
/// Model is Parsed using Utility class NutritionValuesParser
/// </summary>
public class NutritionValuesModel
{
    public string FoodId { get; set; }
    public string Description { get; set; }
    public string FoodCategory { get; set; }
    public double PortionSize { get; set; }
    public string PortionSizeUnit { get; set; }
    public LabelNutrients LabelNutrients { get; set; }
}

public class LabelNutrients
{
    public NutrientValue Fat { get; set; }
    public NutrientValue SaturatedFat { get; set; }
    public NutrientValue TransFat { get; set; }
    public NutrientValue Cholesterol { get; set; }
    public NutrientValue Sodium { get; set; }
    public NutrientValue Carbohydrates { get; set; }
    public NutrientValue Fiber { get; set; }
    public NutrientValue Sugars { get; set; }
    public NutrientValue Protein { get; set; }
    public NutrientValue Calcium { get; set; }
    public NutrientValue Iron { get; set; }
    public NutrientValue Calories { get; set; }
}

public class NutrientValue
{
    public double Value { get; set; }
}