using System;

namespace SensateIoT.SmartEnergy.Dsmr.Data.DTO
{
	public class EnergyDataPoint
	{
		public DateTime Timestamp { get; set; }
		public decimal EnergyProduction { get; set; }
		public decimal EnergyUsage { get; set; }
		public decimal GasFlow { get; set; }
	}
}
