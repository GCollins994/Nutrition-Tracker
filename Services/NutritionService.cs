using Nutrition_Tracker.Models;

namespace Nutrition_Tracker.Services;

public class NutritionService
{
    private readonly ILogger<NutritionService> _logger;
    private readonly NutritionApiService _nutritionApiService;
    
    public NutritionService(ILogger<NutritionService> logger, NutritionApiService nutritionApiService)
    {
        _logger = logger;
        _nutritionApiService = nutritionApiService;
    }
    
    /// <summary>
    /// Search for food items using the NutritionApiService, which calls the Food Data Central API.
    /// </summary>
    /// <param name="searchTerm">Keyword</param>
    /// <param name="itemsPerPage">50/100/150/200</param>
    /// <param name="pageNumber"></param>
    /// <returns>List of items in Json</returns>
    public async Task<FoodSearchModel> SearchFood(string searchTerm, Enums.SearchItemsPerPage itemsPerPage, int pageNumber)
    {
        try
        {
            return await _nutritionApiService.SearchFoodApiRequest(searchTerm, itemsPerPage, pageNumber);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception occurred while searching for food: {e.Message}", e);
            throw;
        }
    }
    
    public async Task<NutritionValuesModel> GetNutritionValues(string foodId)
    {
        try
        {
            return await _nutritionApiService.GetNutritionValueApiRequest(foodId);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception occurred while fetching nutrition values: {e.Message}", e);
            throw;
        }
    }
}