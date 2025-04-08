using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using API.Data;

namespace API
{
    public class Program
    {
        
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // Obtiene el contexto de datos
                    var context = services.GetRequiredService<DataContext>();

                    // Aplica las migraciones pendientes para asegurar la sincronización de la base de datos
                    await context.Database.MigrateAsync();

                    // Llama a la función de seeding
                    await Seed.SeedUsersAsync(context);
                }
                catch (Exception ex)
                {
                    // Maneja y registra cualquier excepción ocurrida en el proceso de seeding
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ocurrió un error durante el seeding de la base de datos.");
                }
            }

            // Ejecuta la aplicación
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
