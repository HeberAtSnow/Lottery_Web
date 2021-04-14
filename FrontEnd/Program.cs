using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace FrontEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("[{prefix}]: Booting application.",
                    LogPrefix.Startup);

                CreateHostBuilder(args).Build().Run();

                Log.Information("[{prefix}]: Application terminated gracefully.",
                    LogPrefix.Startup);
            }
            catch (Exception ex)
            {
                Log.Fatal("[{prefix}]: Application terminated unexpectedly. Reason: {ex}",
                    LogPrefix.Startup, ex);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
