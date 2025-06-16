using Nutrition_Tracker.Models;

namespace Nutrition_Tracker.Interfaces;

public interface INutritionValueParser
{
    NutritionValuesModel ParseNutrition(string json);
}