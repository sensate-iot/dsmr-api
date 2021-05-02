using System;

namespace SensateIoT.SmartEnergy.Dsmr.Data.Models
{
	public class User
	{
		public Guid Id { get; set; }
		public Guid CustomerId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public long Msisdn { get; set; }
		public bool Enabled { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
