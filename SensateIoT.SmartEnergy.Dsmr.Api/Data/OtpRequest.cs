using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Data
{
	public class OtpRequest
	{
		[JsonRequired, JsonProperty("email")]
		public string Email;
	}
}
