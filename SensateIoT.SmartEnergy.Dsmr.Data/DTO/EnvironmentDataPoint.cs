using System;

namespace SensateIoT.SmartEnergy.Dsmr.Data.DTO
{
	public class EnvironmentDataPoint
	{
		public DateTime Timestamp { get; set; }
		public decimal InsideTemperature { get; set; }
		public decimal OutsideAirTemperature { get; set; }
		public decimal Pressure { get; set; }
		public decimal RH { get; set; }
	}
}