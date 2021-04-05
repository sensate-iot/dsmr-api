using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Data.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract
{
	public interface IOlapRepository : IDisposable
	{
		Task<IEnumerable<EnergyDataPoint>> LookupPowerDataPerHourAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct);
		Task<IEnumerable<EnergyDataPoint>> LookupPowerDataPerDayAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct);
		Task<IEnumerable<EnvironmentDataPoint>> LookupEnvironmentDataPerHourAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct);
		Task<IEnumerable<EnvironmentDataPoint>> LookupEnvironmentDataPerDayAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct);
		Task<DataPoint> LookupLastDataPointAsync(int sensorId, CancellationToken ct);
		Task<WeeklyHigh> LookupWeeklyHighAsync(int sensorId, CancellationToken ct);
		Task<IEnumerable<DataPoint>> LookupDataPointsAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct);
	}
}
