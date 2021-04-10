using System;

namespace SensateIoT.SmartEnergy.Dsmr.Data.Models
{
	public class EnvironmentDailyAggregate
	{
		public DateTime Date { get; set; }
		public decimal InsideTemperature { get; set; }
		public decimal OutsideAirTemperature { get; set; }
		public decimal Pressure { get; set; }
		public decimal RH { get; set; }
	}
}