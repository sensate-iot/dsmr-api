using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Data.DTO
{
	public class WeeklyHigh
	{
		[JsonProperty("powerUsage")]
		public decimal PowerUsage { get; set; }
		[JsonProperty("powerProduction")]
		public decimal PowerProduction { get; set; }
		[JsonProperty("gasFlow")]
		public decimal? GasFlow { get; set; }
		[JsonProperty("insideTemperature")]
		public decimal? Temperature { get; set; }
		[JsonProperty("outsideAirTemperatur")]
		public decimal? OutsideAirTemperature { get; set; }
	}
}