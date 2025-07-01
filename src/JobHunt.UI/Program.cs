using JobHunt.Core.Domain.Entities;
using JobHunt.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using JobHunt.UI;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddService();
    builder.Services.AddRepository();
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    });
    builder.Services.AddIdentity<JobHunter, ApplicationRole>(opt =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // Reduce the password complexity for development purposes
            opt.Password.RequiredLength = 5;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredUniqueChars = 1;
            opt.Password.RequireDigit = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireLowercase = true;
        }
    })
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddUserStore<UserStore<JobHunter, ApplicationRole, ApplicationDbContext, Guid>>()
        .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

    builder.Services.AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration.GetValue("Jwt:Issuer", ""),
                ValidateAudience = true,
                ValidAudience = builder.Configuration.GetValue("Jwt:Audience", ""),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(
                        builder.Configuration.GetValue("Jwt:Key", "")
                    )
                )
            };
        });

    builder.Services.AddAuthorization();
    builder.Services.AddControllers(option =>
    {
        var authorizationPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        option.Filters.Add(new AuthorizeFilter(authorizationPolicy));
    });

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policyBuilder =>
        {
            policyBuilder
            .WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
    });

    builder.Services.AddSerilog((services, lc) =>
    {
        lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .WriteTo.Console();
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseRouting();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}



public partial class Program { }
