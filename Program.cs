using Nutrition_Tracker;
using Nutrition_Tracker.Interfaces;
using Nutrition_Tracker.Services;
using Nutrition_Tracker.Utilities;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Load user secrets in development environment
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Add services to the container.
builder.Services.AddScoped<NutritionService>();
builder.Services.AddSingleton<NutritionApiService>();
builder.Services.AddSingleton<INutritionValueParser, NutritionValueParser>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<NutritionApiService>();

// Configure API keys - User Secrets
builder.Services.Configure<ApiKeys>(builder.Configuration.GetSection("ApiKeys"));

// Add Controllers
builder.Services.AddControllers();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/NutritionLog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

