using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Data.DTO
{
	public class User
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }
		[JsonProperty("firstName")]
		public string FirstName { get; set; }
		[JsonProperty("lastName")]
		public string LastName { get; set; }
		[JsonProperty("email")]
		public string Email { get; set; }
		[JsonIgnore]
		public bool Enabled { get; set; }
		[JsonProperty("timestamp")]
		public DateTime Timestamp { get; set; }

		[JsonProperty("devices")]
		public IEnumerable<Device> Devices { get; set; }
	}
}