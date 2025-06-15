using JobHunt.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JobHunt.Infrastructure.DatabaseContext;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<JobFilter> JobFilters { get; set; }
    public virtual DbSet<Job> Jobs { get; set; }
    public virtual DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Model builder for JobFilter
        modelBuilder.Entity<JobFilter>()
            .Property(table => table.Languages)
            .HasStringArrayToStringConversion();
        modelBuilder.Entity<JobFilter>()
            .Property(table => table.SoftSkills)
            .HasStringArrayToStringConversion();
        modelBuilder.Entity<JobFilter>()
            .Property(table => table.TechnicalKnowledge)
            .HasStringArrayToStringConversion();
        modelBuilder.Entity<JobFilter>()
            .Property(table => table.Tools)
            .HasStringArrayToStringConversion();
        // Model builder for Job
        modelBuilder.Entity<Job>().Property(table => table.Requirements)
            .HasStringArrayToStringConversion();

        // Model builder for Projects entity
        modelBuilder.Entity<Project>().Property(entity => entity.TechnologiesOrSkills)
            .HasStringArrayToStringConversion();
        modelBuilder.Entity<Project>().Property(entity => entity.Features)
            .HasStringArrayToStringConversion();

        // Model builder for Profile entity
        modelBuilder.Entity<Profile>().Property(entity => entity.Awards)
            .HasStringArrayToStringConversion();
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