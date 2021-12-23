namespace SensateIoT.SmartEnergy.Dsmr.Data.Models
{
	public class EnergyHourlyAggregate
	{
		public int Hour { get; set; }
		public decimal AveragePowerUsage { get; set; }
		public decimal AveragePowerProduction { get; set; }
		public decimal AverageGasFlow { get; set; }
	}
}
