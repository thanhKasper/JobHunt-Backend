using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.ServiceContracts;
using JobHunt.Core.Services;
using JobHunt.Infrastructure.DatabaseContext;
using JobHunt.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IJobFilterService, JobFilterService>();
builder.Services.AddScoped<IJobFilterRepository, JobFilterRepositories>();
builder.Services.AddScoped<IJobViewService, JobViewService>();
builder.Services.AddScoped<IJobViewRepository, JobViewRepository>();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapControllers();
app.Run();
