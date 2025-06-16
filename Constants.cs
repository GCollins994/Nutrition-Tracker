namespace Nutrition_Tracker;

public class Constants
{
    public const string fdcBaseUrlPrefix = "https://api.nal.usda.gov/fdc/v1/"; //Base URL for the Food Data Central API
}

/// <summary>
/// Api Keys Class to store the API keys saved in User Secrets
/// </summary>
public class ApiKeys
{
    public string FdcApiKey { get; set; }
}

public class CacheKeys
{
    public const string FoodSearchCachePrefix = "FoodSearch_";
    public const string NutritionValuesCachePrefix = "NutritionValues_";
}