using System;

namespace SensateIoT.SmartEnergy.Dsmr.Data.Models
{
	public class Device
	{
		public int Id { get; set; }
		public Guid OnboardingToken { get; set; }
		public string ServiceName { get; set; }
		public bool Enabled { get; set; }
		public string PowerSensorId { get; set; }
		public string GasSensorId { get; set; }
		public string EnvironmentSensorId { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
