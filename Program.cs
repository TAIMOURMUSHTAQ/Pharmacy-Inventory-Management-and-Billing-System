using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pharmacy.Data;

namespace Pharmacy.Win;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var connectionString = config.GetConnectionString("PharmacyDb")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=PharmacyDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        var options = new DbContextOptionsBuilder<PharmacyContext>()
            .UseSqlServer(connectionString)
            .Options;

        using (var db = new PharmacyContext(options))
        {
            DbInitializer.MigrateAndSeed(db);
        }

        Application.Run(new UI.SplashForm(options));
    }
}