using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

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

        public TestTrackDBContext() { }
        public TestTrackDBContext(string connString) : base(connString) { }

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestSuite>()
                .HasRequired(p => p.Team)
                .WithOptional(p => p.TestSuite);

            modelBuilder.Entity<Iteration>()
                .HasMany(p => p.TestPlans)
                .WithRequired(p => p.Iteration)
                .WillCascadeOnDelete(false);
        }
    }

    public class TestTrackContextCustomInitializer : IDatabaseInitializer<TestTrackDBContext>
    {
        public void InitializeDatabase(TestTrackDBContext context)
        {
            if (context.Database.Exists() && context.Database.CompatibleWithModel(true)) return;

            context.Database.Delete();
            context.Database.Create();

            context.Projects.AddOrUpdate(p => p.Title,
                new Project
                {
                    ProjectID = 1,
                    Title = "Doppler",
                    Description = "Create, send, analyze & optimize your Email Marketing campaigns in a effective way. Find out more about the easiest Email Marketing app ever!",
                    CreatedOn = DateTime.Now
                },
                new Project
                {
                    ProjectID = 2,
                    Title = "Lander",
                    Description = "Lander lets you create beautiful landing pages for your social media, email and online marketing campaigns using an easy step-by-step process.",
                    CreatedOn = DateTime.Now
                }
            );
            context.Teams.AddOrUpdate(p => p.Title,
                new Team
                {
                    TeamID = 1,
                    Title = "Doppler QA",
                    ProjectID = 1,
                    CreatedOn = DateTime.Now
                },
                new Team
                {
                    TeamID = 2,
                    Title = "Lander QA",
                    ProjectID = 2,
                    CreatedOn = DateTime.Now
                }
            );
            context.TestSuites.AddOrUpdate(p => p.Title,
                new TestSuite
                {
                    TeamID = 1,
                    Title = "Doppler Front End TS",
                    CreatedOn = DateTime.Now
                },
                new TestSuite
                {
                    TeamID = 2,
                    Title = "Lander Front End TS",
                    CreatedOn = DateTime.Now
                }
            );
            context.SaveChanges();
        }
    }
}