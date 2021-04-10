using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SensateIoT.SmartEnergy.Dsmr.Data.DTO
{
	public class DataPoint
	{
		[JsonProperty("timestamp")]
		public DateTime Timestamp { get; set; }
		[JsonProperty("powerUsage")]
		public decimal PowerUsage { get; set; }
		[JsonProperty("powerProduction")]
		public decimal PowerProduction { get; set; }
		[JsonProperty("tariff", NullValueHandling = NullValueHandling.Ignore)]
		[JsonConverter(typeof(StringEnumConverter))]
		public Tariff? Tariff { get; set; }
		[JsonProperty("energyUsage")]
		public decimal EnergyUsage { get; set; }
		[JsonProperty("energyProduction")]
		public decimal EnergyProduction { get; set; }
		[JsonProperty("gasFlow")]
		public decimal? GasFlow { get; set; }
		[JsonProperty("gasUage")]
		public decimal? GasUsage { get; set; }
		[JsonProperty("outsideAirTemperature")]
		public decimal? OutsideAirTemperature { get; set; }
		[JsonProperty("temperature")]
		public decimal? Temperature { get; set; }
		[JsonProperty("pressure")]
		public decimal? Pressure { get; set; }
		[JsonProperty("rh")]
		public decimal? RH { get; set; }
	}
}
