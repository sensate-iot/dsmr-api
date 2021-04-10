using System;
using System.Collections.Generic;

namespace SensateIoT.SmartEnergy.Dsmr.Data.DTO
{
	public class User
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public bool Enabled { get; set; }
		public DateTime Timestamp { get; set; }

		public IEnumerable<Device> Devices { get; set; }
	}
}