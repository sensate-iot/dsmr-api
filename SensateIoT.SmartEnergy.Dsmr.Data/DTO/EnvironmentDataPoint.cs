using System;
using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Data.DTO
{
	public class EnvironmentDataPoint
	{
		[JsonProperty("timestamp")]
		public DateTime Timestamp { get; set; }
		[JsonProperty("insideTemperature")]
		public decimal InsideTemperature { get; set; }
		[JsonProperty("outsideAirTemperature")]
		public decimal OutsideAirTemperature { get; set; }
		[JsonProperty("pressure")]
		public decimal Pressure { get; set; }
		[JsonProperty("rh")]
		public decimal RH { get; set; }
	}
}