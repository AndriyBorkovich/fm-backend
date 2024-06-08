using FootballManager.Persistence.DatabaseContext;
using FootballManager.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.API.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IServiceProvider"/>
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Run unapplied EF Core migrations for SQL Server DB
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void MigrateDatabase(this IServiceProvider serviceProvider)
        {
            using var container = serviceProvider.CreateScope();
            var dbContext = container.ServiceProvider.GetService<FootballManagerContext>();
            dbContext!.Database.Migrate();
        }

        /// <summary>
        /// Seed DB with initial data
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void SeedDatabase(this IServiceProvider serviceProvider)
        {
            using var container = serviceProvider.CreateScope();
            var seeder = container.ServiceProvider.GetService<IEntitiesSeeder>()!;
            seeder.Run().Wait();
        }
    }
}
