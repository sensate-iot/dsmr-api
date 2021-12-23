using System;
using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Data.DTO
{
	public class Device
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonIgnore]
		public string ServiceName { get; set; }
		[JsonProperty("enabled")]
		public bool Enabled { get; set; }
		[JsonProperty("hasEnvironmentSensor")]
		public bool HasEnvironmentSensor { get; set; }
		[JsonProperty("hasGasSensor")]
		public bool HasGasSensor { get; set; }
		[JsonProperty("hasSolarCells")]
		public bool HasSolarCells { get; set; }
		[JsonProperty("timestamp")]
		public DateTime Timestamp { get; set; }
	}
}