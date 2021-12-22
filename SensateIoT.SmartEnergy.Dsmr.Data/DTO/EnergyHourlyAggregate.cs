using System;
using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Data.DTO
{
	public class EnergyHourlyAggregate
	{
		[JsonProperty("hour")]
		public DateTime Hour { get; set; }
		[JsonProperty("averagePowerUsage")]
		public decimal AveragePowerUsage { get; set; }
		[JsonProperty("averagePowerProduction")]
		public decimal AveragePowerProduction { get; set; }
		[JsonProperty("averageGasFlow")]
		public decimal AverageGasFlow { get; set; }
	}
}
