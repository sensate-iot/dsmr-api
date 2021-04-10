using System;

namespace SensateIoT.SmartEnergy.Dsmr.Data.Models
{
	public class ProductToken
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public Guid Token { get; set; }
		public bool Enabled { get; set; }
		public DateTime Timestamp { get; set; }
	}
}