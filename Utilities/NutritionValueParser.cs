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
    // Nutrient IDs used by the FDC API for non-branded foods (Foundation, SR Legacy, Survey)
    private static class NutrientId
    {
        public const int Protein = 1003;
        public const int Fat = 1004;
        public const int Carbohydrates = 1005;
        public const int Calories = 1008;
        public const int Fiber = 1079;
        public const int Calcium = 1087;
        public const int Iron = 1089;
        public const int Sodium = 1093;
        public const int SaturatedFat = 1258;
        public const int TransFat = 1257;
        public const int Cholesterol = 1253;
        public const int Sugars = 2000;
    }

    public NutritionValuesModel ParseNutrition(string json)
    {
        var jsonNutrition = JObject.Parse(json);

        var foodCategory = (string?)jsonNutrition["brandedFoodCategory"]
                           ?? (jsonNutrition["foodCategory"] is JObject catObj
                               ? (string?)catObj["description"]
                               : (string?)jsonNutrition["foodCategory"])
                           ?? "Unknown";

        var nutritionValuesModel = new NutritionValuesModel
        {
            FoodId = (string?)jsonNutrition["fdcId"] ?? "N/A",
            Description = (string?)jsonNutrition["description"] ?? "N/A",
            FoodCategory = foodCategory,
            PortionSize = (double?)jsonNutrition["servingSize"] ?? 100,
            PortionSizeUnit = (string?)jsonNutrition["servingSizeUnit"] ?? "g",
            LabelNutrients = jsonNutrition["labelNutrients"] != null
                ? ParseLabelNutrients(jsonNutrition["labelNutrients"]!)
                : ParseFoodNutrientsArray(jsonNutrition["foodNutrients"] as JArray)
        };

        return nutritionValuesModel;
    }

    /// <summary>
    /// Parses labelNutrients block — present on branded foods.
    /// </summary>
    private static LabelNutrients ParseLabelNutrients(JToken token)
        => token.ToObject<LabelNutrients>() ?? new LabelNutrients();

    /// <summary>
    /// Parses foodNutrients array — present on Foundation, SR Legacy, and Survey foods.
    /// Each element has a nutrient.id and an amount field.
    /// </summary>
    private static LabelNutrients ParseFoodNutrientsArray(JArray? array)
    {
        if (array == null) return new LabelNutrients();

        var lookup = new Dictionary<int, double>();
        foreach (var item in array)
        {
            var nutrientId = (int?)item["nutrient"]?["id"];
            var amount = (double?)item["amount"];
            if (nutrientId.HasValue && amount.HasValue)
                lookup[nutrientId.Value] = amount.Value;
        }

        return new LabelNutrients
        {
            Calories = NutrientValue(lookup, NutrientId.Calories),
            Protein = NutrientValue(lookup, NutrientId.Protein),
            Fat = NutrientValue(lookup, NutrientId.Fat),
            SaturatedFat = NutrientValue(lookup, NutrientId.SaturatedFat),
            TransFat = NutrientValue(lookup, NutrientId.TransFat),
            Cholesterol = NutrientValue(lookup, NutrientId.Cholesterol),
            Sodium = NutrientValue(lookup, NutrientId.Sodium),
            Carbohydrates = NutrientValue(lookup, NutrientId.Carbohydrates),
            Fiber = NutrientValue(lookup, NutrientId.Fiber),
            Sugars = NutrientValue(lookup, NutrientId.Sugars),
            Calcium = NutrientValue(lookup, NutrientId.Calcium),
            Iron = NutrientValue(lookup, NutrientId.Iron),
        };
    }

    private static Models.NutrientValue? NutrientValue(Dictionary<int, double> lookup, int id)
        => lookup.TryGetValue(id, out var val) ? new Models.NutrientValue { Value = val } : null;
}