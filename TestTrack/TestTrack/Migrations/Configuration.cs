namespace TestTrack.Migrations
{
    using TestTrack.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TestTrack.Models.TestTrackDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TestTrack.Models.TestTrackDBContext context)
        {   
            context.Projects.AddOrUpdate(
                p => p.Title,
                new Project
                {
                    ProjectID = 1,
                    Title = "COPsync",
                    Description = "COPsync operates the nation's largest law enforcement real-time, information sharing, communication and data interoperability network.",
                    CreatedOn = DateTime.Now
                },
                new Project
                {
                    ProjectID = 2,
                    Title = "Doppler",
                    Description = "Create, send, analyze & optimize your Email Marketing campaigns in a effective way. Find out more about the easiest Email Marketing app ever!",
                    CreatedOn = DateTime.Now
                },
                new Project
                {
                    ProjectID = 3,
                    Title = "Lander",
                    Description = "Lander lets you create beautiful landing pages for your social media, email and online marketing campaigns using an easy step-by-step process.",
                    CreatedOn = DateTime.Now
                }
            );
        }
    }
}
