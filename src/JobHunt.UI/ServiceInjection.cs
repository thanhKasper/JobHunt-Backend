using JobHunt.Core.ServiceContracts;
using JobHunt.Core.Services;

namespace JobHunt.UI;

public static class AddServiceInjection
{
    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddScoped<IJobFilterService, JobFilterService>();
        services.AddScoped<IJobViewService, JobViewService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IJobFilterByUserService, JobFilterByUserService>();
        services.AddTransient<IJwtService, JwtService>();
        services.AddScoped<IMajorService, MajorService>();
        services.AddScoped<IEducationService, EducationService>();
        return services;
    }
}