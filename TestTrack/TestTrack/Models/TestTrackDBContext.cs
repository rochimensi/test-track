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
    }
}