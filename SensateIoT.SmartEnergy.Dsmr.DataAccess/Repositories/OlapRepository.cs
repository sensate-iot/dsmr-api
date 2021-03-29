using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Data.Settings;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Repositories
{
	public sealed class OlapRepository : AbstractRepository, IOlapRepository
	{
		private const string DsmrApi_SelectHourlyPowerData = "DsmrApi_SelectHourlyPowerData";

		public OlapRepository(AppSettings settings) : base(new SqlConnection(settings.OlapConnectionString))
		{
		}

		public async Task<IEnumerable<EnergyDataPoint>> LookupEnergyDataPerHour(int sensorId, DateTime start, DateTime end, CancellationToken ct)
		{
			var energyData = await this.QueryAsync<Data.Models.EnergyDataPoint>(DsmrApi_SelectHourlyPowerData,
				"@sensorId", sensorId,
				"@start", start,
				"@end", end).ConfigureAwait(false);

			return energyData.Select(x => new EnergyDataPoint {
				Timestamp = createTimestamp(x.Date, x.Hour),
				EnergyProduction = x.EnergyProduction,
				EnergyUsage = x.EnergyUsage,
				GasFlow = x.GasFlow,
				InsideTemperature = x.InsideTemperature
			});
		}

		private static DateTime createTimestamp(DateTime timestamp, int hour)
		{
			return timestamp.AddHours(hour);
		}
	}
}