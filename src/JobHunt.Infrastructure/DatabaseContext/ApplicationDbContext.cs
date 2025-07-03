using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        
        // Model builder for Job
        modelBuilder.Entity<Job>().Property(table => table.MatchingRequirements)
            .HasStringArrayToStringConversion();

        // Model builder for Projects entity

        // Model builder for Profile entity
        modelBuilder.Entity<JobHunter>().Property(entity => entity.Awards)
            .HasStringArrayToStringConversion();

        new JobFieldEntityTypeConfiguration().Configure(modelBuilder.Entity<JobField>());
        new JobLevelEntityTypeConfiguration().Configure(modelBuilder.Entity<JobLevel>());
        new MajorEntityTypeConfiguration().Configure(modelBuilder.Entity<Major>());
        new EducationEntityTypeConfiguration().Configure(modelBuilder.Entity<Education>());
    }
}

public static class PropertyBuilderExtension
{
    public static PropertyBuilder HasStringArrayToStringConversion(this PropertyBuilder<List<string>?> propertyBuilder)
    {
        return propertyBuilder.HasConversion(
            v => v == null || v.Count == 0 ? null : string.Join(',', v),
            v => v == null ? null : v.Split(',', StringSplitOptions.None).ToList(),
            new ValueComparer<List<string>?>(
                (c1, c2) => (c1 ?? new List<string>()).OrderBy(x => x).SequenceEqual((c2 ?? new List<string>()).OrderBy(x => x)),
                c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c == null ? new List<string>() : c.ToList()
            )
        );
    }
}