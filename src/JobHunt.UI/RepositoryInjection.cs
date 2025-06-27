using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Infrastructure.Repositories;

namespace JobHunt.UI;

public static class RepositoryInjection
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IJobFilterRepository, JobFilterRepository>();
        services.AddScoped<IJobViewRepository, JobViewRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        return services;
    }
}