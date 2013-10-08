using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace TestTrack.Models
{
    public class TestTrackDBContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Iteration> Iterations { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TestSuite> TestSuites { get; set; }
        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<TestPlan> TestPlans { get; set; }
        public DbSet<TestRun> TestRuns { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Defect> Defects { get; set; }

        public override int SaveChanges()
        {
            foreach (var auditableEntity in ChangeTracker.Entries<IAuditable>())
            {
                if (auditableEntity.State == EntityState.Added || auditableEntity.State == EntityState.Modified)
                {
                    // Populate created date column for newly added record.
                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.CreatedOn = DateTime.Now;
                    }
                    else
                    {
                        // Modify updated date.
                        auditableEntity.Entity.LastModified = DateTime.Now;

                        // Make sure that code is not inadvertly modifying created date column.
                        auditableEntity.Property(p => p.CreatedOn).IsModified = false;
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}