using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Data.Models;

using DataPoint = SensateIoT.SmartEnergy.Dsmr.Data.DTO.DataPoint;
using DeviceCapability = SensateIoT.SmartEnergy.Dsmr.Data.DTO.DeviceCapability;
using EnergyDataPoint = SensateIoT.SmartEnergy.Dsmr.Data.DTO.EnergyDataPoint;
using EnergyHourlyAggregate = SensateIoT.SmartEnergy.Dsmr.Data.DTO.EnergyHourlyAggregate;
using EnvironmentDataPoint = SensateIoT.SmartEnergy.Dsmr.Data.DTO.EnvironmentDataPoint;
using GroupedPowerData = SensateIoT.SmartEnergy.Dsmr.Data.DTO.GroupedPowerData;
using WeeklyHigh = SensateIoT.SmartEnergy.Dsmr.Data.DTO.WeeklyHigh;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract
{
	public interface IOlapRepository : IDisposable
	{
		Task<IEnumerable<EnergyDataPoint>> LookupPowerDataPerHourAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct);
		Task<IEnumerable<EnergyDataPoint>> LookupPowerDataPerDayAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct);
		Task<IEnumerable<EnvironmentDataPoint>> LookupEnvironmentDataPerHourAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct);
		Task<IEnumerable<EnvironmentDataPoint>> LookupEnvironmentDataPerDayAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct);
		Task<EnergyUsageData> LookupEnergyDataAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct);
		Task<IEnumerable<GroupedPowerData>> LookupPowerDataByHour(int sensorId, DateTime start, DateTime end, CancellationToken ct);
		Task<DataPoint> LookupLastDataPointAsync(int sensorId, CancellationToken ct);
		Task<WeeklyHigh> LookupWeeklyHighAsync(int sensorId, CancellationToken ct);
		Task<IEnumerable<DataPoint>> LookupDataPointsAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct);
		Task<DeviceCapability> LookupDeviceCapabilities(int sensorId, CancellationToken ct);
		Task<IEnumerable<EnergyHourlyAggregate>> LookupHourlyEnergyAggregates(int sensorId, CancellationToken ct);
	}
}
