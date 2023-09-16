using Backend.Configurations;
using Backend.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Backend
{
    public static class AppExtensions
    {
        /// <summary>
        /// used for adding a onetime mddleware for data seeding
        /// </summary>
        /// <param name="app"></param>
        public async static Task RunAppSeederAsync(this IApplicationBuilder app, IServiceProvider sp)
        {
            // Perform data seeding at application startup
            var dataSeeder = sp.GetRequiredService<DataSeederManager>();

            await dataSeeder.CleanDataAsync();//remove all drivers data from db

            await dataSeeder.Seed10RandomDrivers();//then insert 10 drivers with random data

        }

        /// <summary>
        /// used to add an exception middleware for handling the global exceptions of the application
        /// </summary>
        /// <param name="app"></param>
        public static void UseCustomeExceptionMiddleware(this IApplicationBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                try
                {
                    await next(ctx);
                }
                catch (Exception ex)
                {
                    ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    ctx.Response.ContentType = "application/json";
                    var res = new { success = false, message = ex.Message };
                    await ctx.Response.WriteAsync(JsonSerializer.Serialize(res));
                }
            });
        }

        /// <summary>
        /// used to add a one time migration middleware for managng the database migration at the startup of the application
        /// </summary>
        /// <param name="app"></param>
        public static void UseMigrationsMiddleware(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var settings = scope.ServiceProvider.GetRequiredService<IOptions<SeederSettings>>().Value;

                if (settings.DeleteDataBaseOrRestart)
                    db.Database.EnsureDeleted();

                if (db.Database.GetPendingMigrations().Any())
                {
                    db.Database.Migrate();
                }
            }
        }
    }
}
