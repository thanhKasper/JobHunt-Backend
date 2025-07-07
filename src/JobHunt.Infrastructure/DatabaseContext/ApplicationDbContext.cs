using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Infrastructure.DatabaseContext;

public class ApplicationDbContext(DbContextOptions options) :
    IdentityDbContext<JobHunter, ApplicationRole, Guid>(options)
{
    public virtual DbSet<JobFilter> JobFilters { get; set; }
    public virtual DbSet<Job> Jobs { get; set; }
    public virtual DbSet<Company> Companies { get; set; }
    public virtual DbSet<JobHunter> JobHunters { get; set; }
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<JobField> JobFields { get; set; }
    public virtual DbSet<JobLevel> JobLevels { get; set; }
    public virtual DbSet<Major> Majors { get; set; }
    public virtual DbSet<Education> Educations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new JobFieldEntityTypeConfiguration().Configure(modelBuilder.Entity<JobField>());
        new JobLevelEntityTypeConfiguration().Configure(modelBuilder.Entity<JobLevel>());
        new MajorEntityTypeConfiguration().Configure(modelBuilder.Entity<Major>());
        new EducationEntityTypeConfiguration().Configure(modelBuilder.Entity<Education>());
    }
}