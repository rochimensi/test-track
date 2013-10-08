namespace TestTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false, maxLength: 200),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProjectID);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        ProjectID = c.Int(nullable: false),
                        TestSuiteID = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.TeamID)
                .ForeignKey("dbo.TestSuites", t => t.TestSuiteID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.TestSuiteID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.TestSuites",
                c => new
                    {
                        TestSuiteID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.TestSuiteID);
            
            CreateTable(
                "dbo.TestCases",
                c => new
                    {
                        TestCaseID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Description = c.String(),
                        PreConditions = c.String(),
                        Type = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        Method = c.Int(nullable: false),
                        Tags = c.String(),
                        TestSuiteID = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.TestCaseID)
                .ForeignKey("dbo.TestSuites", t => t.TestSuiteID, cascadeDelete: true)
                .Index(t => t.TestSuiteID);
            
            CreateTable(
                "dbo.Steps",
                c => new
                    {
                        StepId = c.Int(nullable: false, identity: true),
                        TestCaseId = c.Int(nullable: false),
                        Action = c.String(nullable: false),
                        Result = c.String(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.StepId)
                .ForeignKey("dbo.TestCases", t => t.TestCaseId, cascadeDelete: true)
                .Index(t => t.TestCaseId);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        ResultID = c.Int(nullable: false, identity: true),
                        TestCaseID = c.Int(nullable: false),
                        TestRunID = c.Int(nullable: false),
                        State = c.Int(nullable: false, defaultValue: 4),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.ResultID)
                .ForeignKey("dbo.TestCases", t => t.TestCaseID, cascadeDelete: true)
                .ForeignKey("dbo.TestRuns", t => t.TestRunID, cascadeDelete: true)
                .Index(t => t.TestCaseID)
                .Index(t => t.TestRunID);
            
            CreateTable(
                "dbo.TestRuns",
                c => new
                    {
                        TestRunID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Closed = c.Boolean(nullable: false, defaultValue: false),
                        TestPlanID = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.TestRunID)
                .ForeignKey("dbo.TestPlans", t => t.TestPlanID, cascadeDelete: true)
                .Index(t => t.TestPlanID);
            
            CreateTable(
                "dbo.TestPlans",
                c => new
                    {
                        TestPlanID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 500),
                        IterationID = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.TestPlanID)
                .ForeignKey("dbo.Iterations", t => t.IterationID, cascadeDelete: true)
                .Index(t => t.IterationID);
            
            CreateTable(
                "dbo.Iterations",
                c => new
                    {
                        IterationID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        DueDate = c.DateTime(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.IterationID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Defects",
                c => new
                    {
                        DefectID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 500),
                        Severity = c.Int(nullable: false),
                        ResultID = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.DefectID)
                .ForeignKey("dbo.Results", t => t.ResultID, cascadeDelete: true)
                .Index(t => t.ResultID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Defects", new[] { "ResultID" });
            DropIndex("dbo.Iterations", new[] { "ProjectID" });
            DropIndex("dbo.TestPlans", new[] { "IterationID" });
            DropIndex("dbo.TestRuns", new[] { "TestPlanID" });
            DropIndex("dbo.Results", new[] { "TestRunID" });
            DropIndex("dbo.Results", new[] { "TestCaseID" });
            DropIndex("dbo.Steps", new[] { "TestCaseId" });
            DropIndex("dbo.TestCases", new[] { "TestSuiteID" });
            DropIndex("dbo.Teams", new[] { "ProjectID" });
            DropIndex("dbo.Teams", new[] { "TestSuiteID" });
            DropForeignKey("dbo.Defects", "ResultID", "dbo.Results");
            DropForeignKey("dbo.Iterations", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.TestPlans", "IterationID", "dbo.Iterations");
            DropForeignKey("dbo.TestRuns", "TestPlanID", "dbo.TestPlans");
            DropForeignKey("dbo.Results", "TestRunID", "dbo.TestRuns");
            DropForeignKey("dbo.Results", "TestCaseID", "dbo.TestCases");
            DropForeignKey("dbo.Steps", "TestCaseId", "dbo.TestCases");
            DropForeignKey("dbo.TestCases", "TestSuiteID", "dbo.TestSuites");
            DropForeignKey("dbo.Teams", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Teams", "TestSuiteID", "dbo.TestSuites");
            DropTable("dbo.Defects");
            DropTable("dbo.Iterations");
            DropTable("dbo.TestPlans");
            DropTable("dbo.TestRuns");
            DropTable("dbo.Results");
            DropTable("dbo.Steps");
            DropTable("dbo.TestCases");
            DropTable("dbo.TestSuites");
            DropTable("dbo.Teams");
            DropTable("dbo.Projects");
        }
    }
}
