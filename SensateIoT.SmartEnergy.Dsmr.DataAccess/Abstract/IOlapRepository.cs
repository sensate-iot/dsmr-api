using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Data.Models;

using EnergyDataPoint = SensateIoT.SmartEnergy.Dsmr.Data.DTO.EnergyDataPoint;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract
{
	public interface IOlapRepository : IDisposable
	{
		Task<IEnumerable<EnergyDataPoint>> LookupEnergyDataPerHourAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct);
		Task<DataPoint> LookupLastDataPointAsync(int sensorId, CancellationToken ct);
	}
}
