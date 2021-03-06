using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using SensateIoT.SmartEnergy.Dsmr.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Data.Models;
using SensateIoT.SmartEnergy.Dsmr.Data.Settings;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

using DataPoint = SensateIoT.SmartEnergy.Dsmr.Data.Models.DataPoint;
using DeviceCapability = SensateIoT.SmartEnergy.Dsmr.Data.DTO.DeviceCapability;
using EnergyDataPoint = SensateIoT.SmartEnergy.Dsmr.Data.DTO.EnergyDataPoint;
using EnergyHourlyAggregate = SensateIoT.SmartEnergy.Dsmr.Data.DTO.EnergyHourlyAggregate;
using EnvironmentDataPoint = SensateIoT.SmartEnergy.Dsmr.Data.DTO.EnvironmentDataPoint;
using GroupedPowerData = SensateIoT.SmartEnergy.Dsmr.Data.DTO.GroupedPowerData;
using WeeklyHigh = SensateIoT.SmartEnergy.Dsmr.Data.Models.WeeklyHigh;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Repositories
{
	[UsedImplicitly]
	public sealed class OlapRepository : AbstractRepository, IOlapRepository
	{
		private const string DsmrApi_SelectHourlyPowerData = "DsmrApi_SelectHourlyPowerDataAverages";
		private const string DsmrApi_SelectHourlyEnvData = "DsmrApi_SelectHourlyEnvironmentDataAverages";
		private const string DsmrApi_SelectLastData = "DsmrApi_SelectLastData";
		private const string DsmrApi_SelectWeeklyHigh = "DsmrApi_SelectWeeklyHigh";
		private const string DsmrApi_SelectDataPoints = "DsmrApi_SelectDataPoints";
		private const string DsmrApi_SelectPowerDataByHour = "DsmrApi_SelectPowerDataByHour";
		private const string DsmrApi_SelectEnergyDataBetween = "DsmrApi_SelectEnergyDataBetween";
		private const string DsmrApi_ComputeDeviceCapabilities = "DsmrApi_ComputeDeviceCapabilities";
		private const string DsmrApi_SelectAverageEnergyDataPerHour = "DsmrApi_SelectAverageEnergyDataPerHour";

		private const string DsmrApi_SelectPowerDailyAverages = "DsmrApi_SelectPowerDailyAverages";
		private const string DsmrApi_SelectEnvironmentDailyAverages = "DsmrApi_SelectEnvironmentDailyAverages";

		public OlapRepository(AppSettings settings) : base(new SqlConnection(settings.OlapConnectionString))
		{
		}

		public async Task<IEnumerable<EnergyDataPoint>> LookupPowerDataPerHourAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct)
		{
			var energyData = await this.QueryAsync<Data.Models.EnergyDataPoint>(DsmrApi_SelectHourlyPowerData, ct,
				"@sensorId", sensorId,
				"@start", start,
				"@end", end).ConfigureAwait(false);

			return energyData.Select(x => new EnergyDataPoint {
				Timestamp = createTimestamp(x.Date, x.Hour),
				EnergyProduction = x.EnergyProduction,
				EnergyUsage = x.EnergyUsage,
				GasFlow = x.GasFlow,
				Tariff = x.Tariff == 1 ? Tariff.Normal : Tariff.Low
			});
		}

		public async Task<IEnumerable<EnergyDataPoint>> LookupPowerDataPerDayAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct)
		{
			var energyData = await this.QueryAsync<Data.Models.EnergyDataPoint>(DsmrApi_SelectPowerDailyAverages,
				ct,
				"@sensorId", sensorId,
				"@start", start,
				"@end", end).ConfigureAwait(false);

			return energyData.Select(x => new EnergyDataPoint {
				Timestamp = x.Date,
				EnergyProduction = x.EnergyProduction,
				EnergyUsage = x.EnergyUsage,
				GasFlow = x.GasFlow
			});
		}

		public async Task<IEnumerable<EnvironmentDataPoint>> LookupEnvironmentDataPerHourAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct)
		{
			var envData = await this.QueryAsync<Data.Models.EnvironmentDataPoint>(DsmrApi_SelectHourlyEnvData,
				ct,
				"@sensorId", sensorId,
				"@start", start,
				"@end", end).ConfigureAwait(false);

			return envData.Select(x => new EnvironmentDataPoint {
				Timestamp = createTimestamp(x.Date, x.Hour),
				InsideTemperature = x.InsideTemperature,
				OutsideAirTemperature = x.OutsideAirTemperature,
				Pressure = x.Pressure,
				RH = x.RH
			});
		}

		public async Task<IEnumerable<EnvironmentDataPoint>> LookupEnvironmentDataPerDayAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct)
		{
			var envData = await this.QueryAsync<EnvironmentDailyAggregate>(DsmrApi_SelectEnvironmentDailyAverages,
				ct,
				"@sensorId", sensorId,
				"@start", start,
				"@end", end).ConfigureAwait(false);

			return envData.Select(x => new EnvironmentDataPoint {
				Timestamp = x.Date,
				InsideTemperature = x.InsideTemperature,
				OutsideAirTemperature = x.OutsideAirTemperature,
				Pressure = x.Pressure,
				RH = x.RH
			});
		}

		public async Task<EnergyUsageData> LookupEnergyDataAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct)
		{
			return await this.QuerySingleAsync<EnergyUsageData>(DsmrApi_SelectEnergyDataBetween,
				ct,
				"@sensorId", sensorId,
				"@start", start,
				"@end", end).ConfigureAwait(false);
		}

		public async Task<IEnumerable<GroupedPowerData>> LookupPowerDataByHour(int sensorId, DateTime start, DateTime end, CancellationToken ct)
		{
			var powerData = await this.QueryAsync<Data.Models.GroupedPowerData>(DsmrApi_SelectPowerDataByHour,
				ct,
				"@sensorId", sensorId,
				"@start", start,
				"@end", end).ConfigureAwait(false);

			return powerData.Select(x => new GroupedPowerData {
				Hour = x.Hour,
				Production = x.Production,
				Usage = x.Usage
			});
		}

		public async Task<Data.DTO.DataPoint> LookupLastDataPointAsync(int sensorId, CancellationToken ct)
		{
			var data = await this.QuerySingleAsync<DataPoint>(DsmrApi_SelectLastData, ct,
				"@sensorId", sensorId).ConfigureAwait(false);

			if(data == null) {
				return null;
			}

			return new Data.DTO.DataPoint {
				EnergyProduction = data.EnergyProduction,
				EnergyUsage = data.EnergyUsage,
				GasFlow = data.GasFlow,
				GasUsage = data.GasUsage,
				OutsideAirTemperature = data.OutsideAirTemperature,
				PowerProduction = data.PowerProduction,
				PowerUsage = data.PowerUsage,
				Tariff = data.Tariff == 1 ? Tariff.Normal : Tariff.Low,
				Pressure = data.Pressure,
				RH = data.RH,
				Temperature = data.Temperature,
				Timestamp = data.Timestamp
			};
		}

		public async Task<Data.DTO.WeeklyHigh> LookupWeeklyHighAsync(int sensorId, CancellationToken ct)
		{
			var data = await this.QuerySingleAsync<WeeklyHigh>(DsmrApi_SelectWeeklyHigh, ct,
			                                                   "@sensorId", sensorId).ConfigureAwait(false);

			if(data == null) {
				return null;
			}

			return new Data.DTO.WeeklyHigh {
				OutsideAirTemperature = data.Oat,
				PowerProduction = data.PowerProduction,
				PowerUsage = data.PowerUsage,
				Temperature = data.Temperature,
				GasFlow = data.GasFlow
			};
		}

		public async Task<IEnumerable<Data.DTO.DataPoint>> LookupDataPointsAsync(int sensorId, DateTime start, DateTime end, CancellationToken ct)
		{
			var data = await this.QueryAsync<DataPoint>(DsmrApi_SelectDataPoints, ct,
			                                            "@sensorId", sensorId,
			                                            "@start", start,
			                                            "@end", end).ConfigureAwait(false);

			return data.Select(x => new Data.DTO.DataPoint {
				EnergyProduction = x.EnergyProduction,
				EnergyUsage = x.EnergyUsage,
				GasFlow = x.GasFlow,
				GasUsage = x.GasUsage,
				Tariff = x.Tariff == 1 ? Tariff.Normal : Tariff.Low,
				OutsideAirTemperature = x.OutsideAirTemperature,
				PowerProduction = x.PowerProduction,
				PowerUsage = x.PowerUsage,
				Pressure = x.Pressure,
				RH = x.RH,
				Temperature = x.Temperature,
				Timestamp = x.Timestamp
			});
		}

		public async Task<DeviceCapability> LookupDeviceCapabilities(int sensorId, CancellationToken ct)
		{
			var data = await this.QuerySingleAsync<Data.Models.DeviceCapability>(DsmrApi_ComputeDeviceCapabilities, ct,
			                                                   "@sensorId", sensorId).ConfigureAwait(false);

			if(data == null) {
				return null;
			}

			return new DeviceCapability {
				HasGasMeter = data.HasGasMeter ?? false,
				HasSolarCells = data.HasSolarCells
			};
		}

		public async Task<IEnumerable<EnergyHourlyAggregate>> LookupHourlyEnergyAggregates(int sensorId, DateTime start, DateTime end, CancellationToken ct)
		{
			var data = await this.QueryAsync<Data.Models.EnergyHourlyAggregate>(DsmrApi_SelectAverageEnergyDataPerHour, ct,
			                                                   "@sensorId", sensorId,
																         "@start", start,
																         "@end", end).ConfigureAwait(false);
			return data?.Select(x => new EnergyHourlyAggregate {
				AverageGasFlow = x.AverageGasFlow,
				AveragePowerUsage = x.AveragePowerUsage,
				AveragePowerProduction = x.AveragePowerProduction,
				Hour = createTimestampAtHour(x.Hour)
			});
		}

		private static DateTime createTimestampAtHour(int hour)
		{
			return new DateTime(DateTime.UtcNow.Year,
			                    DateTime.UtcNow.Month,
			                    DateTime.UtcNow.Day,
			                    hour,
			                    0,
			                    0,
			                    0,
			                    DateTimeKind.Utc);
		}

		private static DateTime createTimestamp(DateTime timestamp, int hour)
		{
			return timestamp.AddHours(hour);
		}
	}
}
