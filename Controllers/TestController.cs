using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nutrition_Tracker.Data;

namespace Nutrition_Tracker.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<TestController> _logger;

    public TestController(AppDbContext context, ILogger<TestController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Test database connection and show table information.
    /// </summary>
    [HttpGet("database-status")]
    public async Task<IActionResult> GetDatabaseStatus()
    {
        try
        {
            //Test if the database can be connected
            var canConnect = await _context.Database.CanConnectAsync();

            if (!canConnect)
            {
                return BadRequest("Cannot connect to database");
            }
            
            //Get table counts to verity our tables exist
            var userCount = await _context.Users.CountAsync();
            var profileCount = await _context.UserProfiles.CountAsync();
            var foodLogCount = await _context.UserFoodLogs.CountAsync();
            
            var result = new
            {
                DatabaseConnected = true,
                Tables = new
                {
                    Users = userCount,
                    UserProfiles = profileCount,
                    UserFoodLogs = foodLogCount
                },
                Message = "Database is working correctly.",
                Timestamp = DateTime.UtcNow
            };
            
            _logger.LogInformation("Database status checked successfully at {Timestamp}", result.Timestamp);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking database status");
            return StatusCode(500, $"Database error: {ex.Message}");
        }
    }

    /// <summary>
    /// Get table information
    /// </summary>
    [HttpGet("table-info")]
    public async Task<IActionResult> GetTableInfo()
    {
        try
        {
            //Get all table names from database
            var entityTypes = _context.Model.GetEntityTypes();
            var tableNames = entityTypes
                .Select(t => t.GetTableName())
                .Where(name => name != null)
                .ToList();

            var result = new
            {
                DatabaseName = _context.Database.GetDbConnection().Database,
                Method = "EF Core Metadata",
                TableCount = tableNames.Count,
                Tables = tableNames.OrderBy(name => name).ToList(),
                IdentityTables = tableNames.Where(t => t.StartsWith("AspNet")).ToList(),
                CustomTables = tableNames.Where(t => !t.StartsWith("AspNet")).ToList(),
                EntityDetails = entityTypes.Select(e => new
                    {
                        EntityName = e.ClrType.Name,
                        TableName = e.GetTableName(),
                        Properties = e.GetProperties().Count()
                    }).ToList()
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting table metadata");
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Test Creating and reading a sample user
    /// </summary>
    [HttpPost("create-test-user")]
    public async Task<IActionResult> CreateTestUser()
    {
        try
        {
            // Check if test user already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == "test@example.com");
            
            if (existingUser != null)
            {
                return BadRequest("Test user already exists.");
            }
            
            //Test (normally would use Identities UserManager)
            var testUser = new Nutrition_Tracker.Entities.ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "test@example.com",
                Email = "test@example.com",
                NormalizedUserName = "TEST@EXAMPLE.COM",
                NormalizedEmail = "TEST@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            
            _context.Users.Add(testUser);
            await _context.SaveChangesAsync();

            var result = new
            {
                Message = "Test created successfully.",
                UserId = testUser.Id,
                Email = testUser.Email,
                Warning = "This is a test"
            };
            
            _logger.LogInformation("Test User Created: {Email}", testUser.Email);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating test user");
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Clean up data
    /// </summary>
    [HttpDelete("cleanup")]
    public async Task<IActionResult> CleanupTestData()
    {
        try
        {
            //Remove test users
            var testUsers = await _context.Users
                .Where(u => u.Email.Contains("test") || u.Email.Contains("example"))
                .ToListAsync();

            if (testUsers.Any())
            {
                _context.Users.RemoveRange(testUsers);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = $"Removed {testUsers.Count} test users.",
                    RemovedEmails = testUsers.Select(u => u.Email).ToList()
                });
            }

            return Ok(new { Message = "No test data to clean up." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during cleanup");
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}