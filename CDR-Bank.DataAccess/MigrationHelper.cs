using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CDR_Bank.DataAccess
{
    public static class MigrationHelper
    {
        public static TContext ApplyMigrationsIfNotEf<TContext>(this TContext context, ILogger? logger = null)
            where TContext : DbContext
        {
            if (IsEfToolRunning())
            {
                logger?.LogInformation("Skipping the migration application, as it was launched via dotnet ef.");
                return context;
            }

            try
            {
                logger?.LogInformation("Application of migrations...");
                context.Database.Migrate();
                logger?.LogInformation("Migrations have been applied successfully.");

                return context;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error when applying migrations.");
                throw;
            }
        }

        public static bool IsEfToolRunning()
        {
            if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_EF") == "1")
                return true;

            var processName = Process.GetCurrentProcess().ProcessName;
            return processName.Contains("ef", StringComparison.OrdinalIgnoreCase);
        }
    }
}
