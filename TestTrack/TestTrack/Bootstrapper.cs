using Castle.Windsor;
using TestTrack.Infrastructure.Ioc;

namespace TestTrack
{ 
    public static class Bootstrapper
	{ 
		public static IWindsorContainer InitializeContainer()
		{
            return new WindsorContainer().Install(new WindsorInstaller());
		}

		public static void Release(IWindsorContainer container)
		{
			container.Dispose();
		}
	}
}
