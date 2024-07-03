using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Persistence.Extensions
{
    public static class MigrationManager
    {
        public static void ApplyMigrations(IServiceProvider serviceProvider, ILogger logger)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                var dbContextTypes = GetDbContextTypes();

                foreach (var dbContextType in dbContextTypes)
                {
                    try
                    {
                        var context = (DbContext)services.GetRequiredService(dbContextType);

                        logger.LogInformation($"Migrating database associated with context {dbContextType.Name}");

                        context.Database.Migrate();

                        logger.LogInformation($"Migrated database associated with context {dbContextType.Name}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"An error occurred while migrating the database associated with context {dbContextType.Name}");
                    }
                }
            }
        }

        private static List<Type> GetDbContextTypes()
        {
            var dbContextTypes = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;

                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray();
                }

                dbContextTypes.AddRange(types.Where(type => type.IsSubclassOf(typeof(DbContext)) && !type.IsAbstract));
            }

            return dbContextTypes;
        }
    }
}
