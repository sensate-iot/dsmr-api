using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Data
{
	public class LogoutRequest
	{
		[JsonProperty("email")]
		public string Email { get; set; }
	}
}
