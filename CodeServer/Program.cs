using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeServer.Data.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CodeServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File("C://Logs" + "/CodeServer/Log-.txt",
                        outputTemplate: "{NewLine}{NewLine} TIME: {Timestamp:HH:mm:ss} {NewLine} TYPE:{Level} {NewLine} MESSAGE:{Message}{NewLine} EXCEPTION: {Exception}",
                        rollingInterval: RollingInterval.Day,
                        shared: true)
            .CreateLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                MigrateDatabase(host);

                host.Run();
               // CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        private static void MigrateDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
            .CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
