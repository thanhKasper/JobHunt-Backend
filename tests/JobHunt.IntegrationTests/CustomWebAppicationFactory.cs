using JobHunt.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Get the assembly that contains your Program class

        // base.ConfigureWebHost(builder);

        builder.UseEnvironment("Test");
        builder.ConfigureServices(
            services =>
            {
                // Remove all EF Core related services
                var descriptorsToRemove = services.Where(d =>
                    d.ServiceType == typeof(ApplicationDbContext) ||
                    d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    d.ServiceType.Name.Contains("EntityFramework") ||
                    d.ServiceType.Name.Contains("DbContext") ||
                    (d.ImplementationType?.Name.Contains("EntityFramework") == true) ||
                    (d.ImplementationType?.Name.Contains("SqlServer") == true)
                ).ToList();

                foreach (var descriptor in descriptorsToRemove)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDbContext>(option =>
                {
                    option.UseInMemoryDatabase("testDB");
                });
            }
        );

    }
}
