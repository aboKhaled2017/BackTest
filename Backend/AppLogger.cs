using Microsoft.AspNetCore.Builder;
using Serilog;
using System.IO;

namespace Backend
{
    public static class AppLogger
    {
        public static void ConfigureLogging(this WebApplicationBuilder builder)
        {

            builder.Host.UseSerilog((ctx, config) =>
            {
                config.ReadFrom.Configuration(ctx.Configuration);
            });


            if (!Directory.Exists("AppLogs"))
            {
                Directory.CreateDirectory("AppLogs");
            }
        }
    }
}
