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
            context.Teams.AddOrUpdate(p => p.Name,
                new Team
                {
                    TeamID = 1,
                    Name = "Doppler QA",
                    ProjectID = 1,
                    CreatedOn = DateTime.Now
                },
                new Team
                {
                    TeamID = 2,
                    Name = "Lander QA",
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
            context.Iterations.AddOrUpdate(p => p.Title,
                new Iteration
                {
                    IterationID = 1,
                    Title = "Sprint 1",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    ProjectID = 1,
                    CreatedOn = DateTime.Now
                },
                new Iteration
                {
                    IterationID = 2,
                    Title = "Sprint 2",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    ProjectID = 1,
                    CreatedOn = DateTime.Now
                }

            );
            context.TestPlans.AddOrUpdate(p => p.Title,
                new TestPlan
                {
                    TestPlanID = 1,
                    Title = "Sanity Testing",
                    Description = "Basic Functionality",
                    IterationID = 1,
                    TeamID = 1,
                    CreatedOn = DateTime.Now
                },
                new TestPlan
                {
                    TestPlanID = 2,
                    Title = "Regression Testing",
                    Description = "All Functionality. This is the test plan description blah blah.",
                    IterationID = 1,
                    TeamID = 1,
                    CreatedOn = DateTime.Now
                },
                new TestPlan
                {
                    TestPlanID = 3,
                    Title = "Sanity Testing",
                    Description = "Basic Functionality",
                    IterationID = 2,
                    TeamID = 1,
                    CreatedOn = DateTime.Now
                },
                new TestPlan
                {
                    TestPlanID = 4,
                    Title = "Functional Testing",
                    Description = "User Story Functionality",
                    IterationID = 2,
                    TeamID = 1,
                    CreatedOn = DateTime.Now
                }
            );

            context.TestRuns.AddOrUpdate(p => p.Title,
                new TestRun
                {
                    TestRunID = 1,
                    Title = "Login test run",
                    TestPlanID = 2,
                    CreatedOn = DateTime.Now
                },
                new TestRun
                {
                    TestRunID = 2,
                    Title = "Home page test run",
                    TestPlanID = 2,
                    CreatedOn = DateTime.Now
                },
                new TestRun
                {
                    TestRunID = 3,
                    Title = "Projects page test run",
                    TestPlanID = 2,
                    CreatedOn = DateTime.Now
                },
                new TestRun
                {
                    TestRunID = 4,
                    Title = "Regression sprint 1",
                    TestPlanID = 2,
                    Closed = true,
                    CreatedOn = DateTime.Now
                },
                new TestRun
                {
                    TestRunID = 5,
                    Title = "Regression sprint 2",
                    TestPlanID = 2,
                    Closed = true,
                    CreatedOn = DateTime.Now
                },
                new TestRun
                {
                    TestRunID = 6,
                    Title = "Regression sprint 3",
                    TestPlanID = 2,
                    Closed = true,
                    CreatedOn = DateTime.Now
                }
            );

            context.TestCases.AddOrUpdate(p => p.Title,
                new TestCase
                {
                    TestCaseID = 1,
                    Title = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt",
                    Description = "Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.",
                    PreConditions = "The user must be logged in the application.",
                    Type = Type.Functional,
                    Priority = Priority.High,
                    Method = Method.Automatable,
                    Tags = "lorem,ipsum,dolor",
                    TestSuiteID = 1,
                    CreatedOn = DateTime.Now
                },
                new TestCase
                {
                    TestCaseID = 2,
                    Title = "Feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril",
                    Description = "Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.",
                    PreConditions = "The user must be logged in the application.",
                    Type = Type.Functional,
                    Priority = Priority.High,
                    Method = Method.Automated,
                    Tags = "lorem,ipsum,dolor",
                    TestSuiteID = 1,
                    CreatedOn = DateTime.Now
                },
                new TestCase
                {
                    TestCaseID = 3,
                    Title = " Nam liber tempor cum soluta nobis eleifend option congue nihil ",
                    Description = "Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.",
                    PreConditions = "The user must be logged in the application.",
                    Type = Type.Regression,
                    Priority = Priority.Low,
                    Method = Method.Automatable,
                    Tags = "lorem,ipsum,dolor",
                    TestSuiteID = 1,
                    CreatedOn = DateTime.Now
                },
                new TestCase
                {
                    TestCaseID = 4,
                    Title = "Eodem modo typi, qui nunc nobis videntur parum clari",
                    Description = "Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.",
                    PreConditions = "The user must be logged in the application.",
                    Type = Type.Functional,
                    Priority = Priority.High,
                    Method = Method.Manual,
                    Tags = "lorem,ipsum,dolor",
                    TestSuiteID = 1,
                    CreatedOn = DateTime.Now
                },
                new TestCase
                {
                    TestCaseID = 5,
                    Title = "Claritas est etiam processus dynamicus, qui sequitur",
                    Description = "Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.",
                    PreConditions = "The user must be logged in the application.",
                    Type = Type.Sanity,
                    Priority = Priority.Medium,
                    Method = Method.Manual,
                    Tags = "lorem,ipsum,dolor",
                    TestSuiteID = 1,
                    CreatedOn = DateTime.Now
                },
                new TestCase
                {
                    TestCaseID = 6,
                    Title = "Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat",
                    Description = "Typi non habent claritatem insitam; est usus legentis in iis qui facit eorum claritatem. Investigationes demonstraverunt lectores legere me lius quod ii legunt saepius.",
                    PreConditions = "The user must be logged in the application.",
                    Type = Type.Functional,
                    Priority = Priority.High,
                    Method = Method.Automatable,
                    Tags = "lorem,ipsum,dolor",
                    TestSuiteID = 1,
                    CreatedOn = DateTime.Now
                },
                new TestCase
                {
                    TestCaseID = 7,
                    Title = "Soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat",
                    Description = "Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.",
                    PreConditions = "The user must be logged in the application.",
                    Type = Type.Sanity,
                    Priority = Priority.Medium,
                    Method = Method.Automated,
                    Tags = "lorem,ipsum,dolor",
                    TestSuiteID = 1,
                    CreatedOn = DateTime.Now
                },
                new TestCase
                {
                    TestCaseID = 8,
                    Title = "Quam nunc putamus parum claram, anteposuerit litterarum formas humanitatis per seacula quarta decima et quinta",
                    Description = "Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.",
                    PreConditions = "The user must be logged in the application.",
                    Type = Type.Regression,
                    Priority = Priority.Low,
                    Method = Method.Automatable,
                    Tags = "lorem,ipsum,dolor",
                    TestSuiteID = 1,
                    CreatedOn = DateTime.Now
                });

            context.Steps.AddOrUpdate(p => p.Action,
                new Step
                {
                    StepId = 1,
                    TestCaseId = 1,
                    Action = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    Result = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    CreatedOn = DateTime.Now
                },
                new Step
                {
                    StepId = 2,
                    TestCaseId = 1,
                    Action = "Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi. Nam liber tempor cum soluta nobis eleifend option",
                    Result = "Lore te feugait nulla facilisi. Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi. Nam liber tempor cum",
                    CreatedOn = DateTime.Now
                },
                new Step
                {
                    StepId = 3,
                    TestCaseId = 2,
                    Action = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    Result = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    CreatedOn = DateTime.Now
                },
                new Step
                {
                    StepId = 4,
                    TestCaseId = 3,
                    Action = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    Result = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    CreatedOn = DateTime.Now
                },
                new Step
                {
                    StepId = 5,
                    TestCaseId = 4,
                    Action = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    Result = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    CreatedOn = DateTime.Now
                },
                new Step
                {
                    StepId = 6,
                    TestCaseId = 5,
                    Action = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    Result = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    CreatedOn = DateTime.Now
                },
                new Step
                {
                    StepId = 7,
                    TestCaseId = 6,
                    Action = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    Result = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    CreatedOn = DateTime.Now
                },
                new Step
                {
                    StepId = 8,
                    TestCaseId = 7,
                    Action = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    Result = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    CreatedOn = DateTime.Now
                },
                new Step
                {
                    StepId = 9,
                    TestCaseId = 8,
                    Action = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    Result = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna",
                    CreatedOn = DateTime.Now
                },
                new Step
                {
                    StepId = 10,
                    TestCaseId = 1,
                    Action = "Id quod mazim placerat lore te feugait nulla facilisi. Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait",
                    Result = "Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi. Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet",
                    CreatedOn = DateTime.Now
                });

            context.Results.AddOrUpdate(p => p.ResultID,
                new Result
                {
                    ResultID = 1,
                    TestCaseID = 1,
                    TestRunID = 2,
                    State = State.Passed,
                    Comments = "Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.",
                    AssignedTo = "Carlitos",
                    CreatedOn = DateTime.Now
                },
                new Result
                {
                    ResultID = 2,
                    TestCaseID = 1,
                    TestRunID = 2,
                    State = State.Failed,
                    Comments = "Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.",
                    AssignedTo = "Ro",
                    CreatedOn = DateTime.Now
                },
                new Result
                {
                    ResultID = 3,
                    TestCaseID = 3,
                    TestRunID = 2,
                    State = State.Passed,
                    Comments = "Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.",
                    AssignedTo = "Mati",
                    CreatedOn = DateTime.Now
                },
                new Result
                {
                    ResultID = 4,
                    TestCaseID = 4,
                    TestRunID = 2,
                    State = State.Failed,
                    Comments = "Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.",
                    AssignedTo = "Marcelo",
                    CreatedOn = DateTime.Now
                },
                new Result
                {
                    ResultID = 5,
                    TestCaseID = 5,
                    TestRunID = 2,
                    State = State.Failed,
                    Comments = "Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.Nobis eleifend option congue nihil imperdiet doming id quod mazim placerat lore te feugait nulla facilisi.",
                    AssignedTo = "Diego",
                    CreatedOn = DateTime.Now
                },
                new Result
                {
                    ResultID = 6,
                    TestCaseID = 6,
                    TestRunID = 2,
                    State = State.Untested,
                    CreatedOn = DateTime.Now
                },
                new Result
                {
                    ResultID = 7,
                    TestCaseID = 7,
                    TestRunID = 2,
                    State = State.Untested,
                    CreatedOn = DateTime.Now
                },
                new Result
                {
                    ResultID = 8,
                    TestCaseID = 8,
                    TestRunID = 2,
                    State = State.Untested,
                    CreatedOn = DateTime.Now
                });

            context.SaveChanges();
        }
    }
}