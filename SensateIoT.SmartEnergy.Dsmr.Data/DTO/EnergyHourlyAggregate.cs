using System;

namespace SensateIoT.SmartEnergy.Dsmr.Data.DTO
{
	public class EnergyHourlyAggregate
	{
		public DateTime Hour { get; set; }
		public decimal AveragePowerUsage { get; set; }
		public decimal AverageGasFlow { get; set; }
	}
}
