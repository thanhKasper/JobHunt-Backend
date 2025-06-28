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

        base.ConfigureWebHost(builder);

        builder.UseEnvironment("Test");
        builder.ConfigureServices(
            services =>
            {
                var descriptor = services.SingleOrDefault(
                    temp => temp.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)
                );

                if (descriptor != null)
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
