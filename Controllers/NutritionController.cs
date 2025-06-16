using Microsoft.AspNetCore.Mvc;
using Nutrition_Tracker.Models;
using Nutrition_Tracker.Services;


namespace Nutrition_Tracker.Controllers;

[ApiController]
[Route("[controller]")]
public class NutritionController : ControllerBase
{

    private readonly ILogger<NutritionController> _logger;
    private readonly HttpClient _httpClient;
    private readonly NutritionService _nutritionService;

    public NutritionController(ILogger<NutritionController> logger, HttpClient httpClient, NutritionService nutritionService)
    {
        _logger = logger;
        _httpClient = httpClient;
        _nutritionService = nutritionService;
    }
    
    /// <summary>
    /// Search in DFC database. Input a search term (Example: "Banana"), Number of Items Per page, and get a list of food items.
    /// </summary>
    /// <param name="searchTerm">Keyword</param>
    /// <param name="itemsPerPage">50/100/150/200</param>
    /// <param name="pageNumber"></param>
    /// <returns>List of items in Json</returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchFood(string searchTerm, Enums.SearchItemsPerPage itemsPerPage = Enums.SearchItemsPerPage.Fifty, int pageNumber = 1)
    {
        try
        {
            var matches = await _nutritionService.SearchFood(searchTerm, itemsPerPage, pageNumber);
            if (matches == null)
            {
                return NotFound();
            }
            return Ok(matches);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception occurred while searching for food: {e.Message}", e);
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    /// <summary>
    /// Endpoint that takes in a Food Item ID and returns the nutrition values for that item. Structure of the response
    /// is defined in NutritionValuesModel, using NutritionValueParser to organize the data.
    /// </summary>
    /// <param name="fdcId">Food Data Central API - Item ID</param>
    /// <returns></returns>
    [HttpGet("itemNutritionValues")]
    public async Task<IActionResult> GetNutritionValues(string fdcId)
    {
        try
        {
            var nutritionValues = await _nutritionService.GetNutritionValues(fdcId);
            if (nutritionValues == null)
            {
                return NotFound();
            }
            return Ok(nutritionValues);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception occurred while fetching nutrition values: {e.Message}", e);
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    
}