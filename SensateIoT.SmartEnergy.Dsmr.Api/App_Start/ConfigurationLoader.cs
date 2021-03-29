using System;
using System.Configuration;

using SensateIoT.SmartEnergy.Dsmr.Data.Settings;

namespace SensateIoT.SmartEnergy.Dsmr.Api
{
	public static class ConfigurationLoader
	{
		public static AppSettings Load()
		{
			return new AppSettings {
				OlapConnectionString = ConfigurationManager.ConnectionStrings["olap"]?.ConnectionString ??
				                       throw new InvalidOperationException("OLAP connection string not configured")
			};
		}
	}
}
