using Newtonsoft.Json.Linq;
using Nutrition_Tracker.Interfaces;
using Nutrition_Tracker.Models;

namespace Nutrition_Tracker.Utilities;

/// <summary>
/// Class to parse the Nutrition Values Fetched from the FDC API, store them into a parsed format and return them to
/// the Controller or used elsewhere
/// </summary>
public class NutritionValueParser : INutritionValueParser
{
    public NutritionValuesModel ParseNutrition(string json)
    {
        var jsonNutrition = JObject.Parse(json);

        var labelNutrientsToken = jsonNutrition["labelNutrients"];
        LabelNutrients labelNutrients = labelNutrientsToken != null
            ? labelNutrientsToken.ToObject<LabelNutrients>()
            : new LabelNutrients(); // fallback if missing, could be null

        var nutritionValuesModel = new NutritionValuesModel
        {
            FoodId = (string?)jsonNutrition["fdcId"] ?? "N/A",
            Description = (string?)jsonNutrition["description"] ?? "N/A",
            FoodCategory = (string?)jsonNutrition["foodCategory"]
                           ?? (string?)jsonNutrition["brandedFoodCategory"]
                           ?? "Unknown",
            PortionSize = (double?)jsonNutrition["servingSize"] ?? 0,
            PortionSizeUnit = (string?)jsonNutrition["servingSizeUnit"] ?? "g",
            LabelNutrients = labelNutrients
        };

        return nutritionValuesModel;
    }
}