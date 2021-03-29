using System.Web.Http;

using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Repositories;

using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace SensateIoT.SmartEnergy.Dsmr.Api
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
			var config = ConfigurationLoader.Load();

			container.RegisterInstance(config);
			container.RegisterType<IOlapRepository, OlapRepository>(new HierarchicalLifetimeManager());

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
