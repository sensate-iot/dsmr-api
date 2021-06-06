using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Data.Models
{
	public class EnergyUsageData
	{
		[JsonProperty("energyUsage")]
		public decimal EnergyUsage { get; set; }
		[JsonProperty("energyProduction")]
		public decimal EnergyProduction { get; set; }
		[JsonProperty("gasUsage")]
		public decimal? GasUsage { get; set; }
	}
}
