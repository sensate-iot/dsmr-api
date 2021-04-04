using System;

namespace SensateIoT.SmartEnergy.Dsmr.Data.Models
{
	public class EnergyDataPoint
	{
		public DateTime Date { get; set; }
		public int Hour { get; set; }
		public decimal EnergyProduction { get; set; }
		public decimal EnergyUsage { get; set; }
		public decimal GasFlow { get; set; }
	}
}
