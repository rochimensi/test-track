using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using TestTrack.Models;

namespace TestTrack.Infrastructure.EF
{
    public interface IDbContext
    {
        DbSet<Project> Projects { get; set; }
        DbSet<Iteration> Iterations { get; set; }
        DbSet<Team> Teams { get; set; }
        DbSet<TestSuite> TestSuites { get; set; }
        DbSet<TestCase> TestCases { get; set; }
        DbSet<Step> Steps { get; set; }
        DbSet<TestPlan> TestPlans { get; set; }
        DbSet<TestRun> TestRuns { get; set; }
        DbSet<Result> Results { get; set; }
        DbSet<Defect> Defects { get; set; }

        DbSet<TEntity> Entity<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        Database Database { get; }

        int SaveChanges();

    }
}
