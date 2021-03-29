using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Data.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract
{
	public interface IOlapRepository : IDisposable
	{
		Task<IEnumerable<EnergyDataPoint>> LookupEnergyDataPerHour(int sensorId, DateTime start, DateTime end, CancellationToken ct);
	}
}
