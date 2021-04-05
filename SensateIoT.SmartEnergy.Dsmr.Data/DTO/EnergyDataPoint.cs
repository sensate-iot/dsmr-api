using System;
using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Data.DTO
{
	public class EnergyDataPoint
	{
		[JsonProperty("timestamp")]
		public DateTime Timestamp { get; set; }
		[JsonProperty("energyProduction")]
		public decimal EnergyProduction { get; set; }
		[JsonProperty("energyUsage")]
		public decimal EnergyUsage { get; set; }
		[JsonProperty("gasFlow")]
		public decimal GasFlow { get; set; }
	}
}
