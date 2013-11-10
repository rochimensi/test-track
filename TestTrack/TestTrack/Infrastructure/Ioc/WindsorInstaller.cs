using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Hiperion.Infrastructure.Automapper;

namespace TestTrack.Infrastructure.Ioc
{
    internal class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(Classes.FromThisAssembly()
                            .BasedOn<IController>()
                            .LifestyleTransient());


            AutomapperConfiguration.Configure(container.Resolve);
        }
    }
}