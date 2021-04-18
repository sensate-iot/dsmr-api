using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Data
{
	public class LoginRequest
	{
		[JsonProperty("otp"), JsonRequired]
		public string Otp { get; set; }
		[JsonProperty("email"), JsonRequired]
		public string Email { get; set; }
	}
}
