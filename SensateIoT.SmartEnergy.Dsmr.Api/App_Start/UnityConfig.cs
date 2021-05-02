using System.Configuration;
using System.Web.Http;
using SensateIoT.SmartEnergy.Dsmr.Api.Adapters;
using SensateIoT.SmartEnergy.Dsmr.Data.Settings;
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

			initializeTwilio(config);

			container.RegisterInstance(config);
			container.RegisterType<IDeviceRepository, DeviceRepository>(new HierarchicalLifetimeManager());
			container.RegisterType<IOlapRepository, OlapRepository>(new HierarchicalLifetimeManager());
			container.RegisterType<IAuthenticationRepository, AuthenticationRepository>(new HierarchicalLifetimeManager());
			container.RegisterType<ISmsAdapter, TwilioAdapter>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }


        private static void initializeTwilio(AppSettings config)
        {
			if(!config.OtpEnabled) {
				return;
			}

			var account = ConfigurationManager.AppSettings["twilioAccountToken"];
			var auth = ConfigurationManager.AppSettings["twilioAuthToken"];

			TwilioAdapter.Init(account, auth);
        }
    }
}
