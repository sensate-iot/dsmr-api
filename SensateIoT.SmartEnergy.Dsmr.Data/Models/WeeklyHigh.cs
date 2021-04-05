namespace SensateIoT.SmartEnergy.Dsmr.Data.Models
{
	public class WeeklyHigh
	{
		public decimal PowerUsage { get; set; }
		public decimal PowerProduction { get; set; }
		public decimal? GasFlow { get; set; }
		public decimal? Temperature { get; set; }
		public decimal? Oat { get; set; }
	}
}