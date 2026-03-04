using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Nutrition_Tracker.Data

{
    /// <summary>
    /// EF Core design-time factory for creating the AppDbContext. EF Core was not able to create an instance of the AppDbContext.
    /// This class is used to create the AppDbContext at design time, such as when running migrations, manually.
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
/*TODO: Add to README
 
 ## Developer Note

This project uses a local SQL Server Express setup for development only.

    If you're using this project:

- Make sure to update your connection string in `appsettings.json`
- Avoid disabling encryption (`Encrypt=False`) unless you're in a trusted dev environment
- Never use this config in production, secure SQL connections are required */
