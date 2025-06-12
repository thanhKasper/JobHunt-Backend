using JobHunt.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Infrastructure.DatabaseContext;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<JobFilter> JobFilters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
    }
}