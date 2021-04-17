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
		[JsonProperty("name")]
		public bool Enabled { get; set; }
		[JsonIgnore]
		public string PowerSensorId { get; set; }
		[JsonIgnore]
		public string GasSensorId { get; set; }
		[JsonIgnore]
		public string EnvironmentSensorId { get; set; }
		[JsonProperty("timestamp")]
		public DateTime Timestamp { get; set; }
	}
}