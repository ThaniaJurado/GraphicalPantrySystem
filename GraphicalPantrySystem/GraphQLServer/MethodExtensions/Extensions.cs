using GraphQLServer.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.MethodExtensions
{
    public static class Extensions
    {
        public static async Task ConfigurateMigrations(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();

            try
            {
                var context = services.GetRequiredService<BlogContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }
    }
}
