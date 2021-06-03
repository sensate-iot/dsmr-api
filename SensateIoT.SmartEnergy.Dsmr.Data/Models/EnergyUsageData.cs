namespace SensateIoT.SmartEnergy.Dsmr.Data.Models
{
	public class EnergyUsageData
	{
		public decimal EnergyUsage { get; set; }
		public decimal EnergyProduction { get; set; }
		public decimal? GasUsage { get; set; }
	}
}
