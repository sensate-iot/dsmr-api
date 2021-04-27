using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Data.DTO
{
	public class GroupedPowerData
	{
		[JsonProperty("hour")]
		public int Hour { get; set; }
		[JsonProperty("usage")]
		public decimal Usage { get; set; }
		[JsonProperty("production")]
		public decimal Production { get; set; }
	}
}
