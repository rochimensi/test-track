using System.Configuration;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Hiperion.Infrastructure.Automapper;
using TestTrack.Infrastructure.EF;
using TestTrack.Models;

namespace TestTrack.Infrastructure.Ioc
{
    internal class WindsorInstaller : IWindsorInstaller
    {

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TestTrackDBContext"].ConnectionString;

            container.Register(

                 Component.For<IDbContext>()
                          .ImplementedBy<TestTrackDBContext>()
                          .LifestylePerWebRequest()
                          .DependsOn(Parameter.ForKey("nameOrConnectionString").Eq(connectionString)),

                 Classes.FromThisAssembly()
                             .BasedOn<IController>()
                             .LifestyleTransient()
             );

            AutomapperConfiguration.Configure(container.Resolve);
        }
    }
}