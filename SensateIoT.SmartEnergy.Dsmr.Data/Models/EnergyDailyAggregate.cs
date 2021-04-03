using System;

namespace SensateIoT.SmartEnergy.Dsmr.Data.Models
{
	public class EnergyDailyAggregate
	{
		public DateTime Date { get; set; }
		public decimal EnergyProduction { get; set; }
		public decimal EnergyUsage { get; set; }
		public decimal GasFlow { get; set; }
	}
}
