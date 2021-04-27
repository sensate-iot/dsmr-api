namespace SensateIoT.SmartEnergy.Dsmr.Data.Models
{
	public class GroupedPowerData
	{
		public int Hour { get; set; }
		public decimal Usage { get; set; }
		public decimal Production { get; set; }
	}
}
