using System;

namespace SensateIoT.SmartEnergy.Dsmr.Data.DTO
{
	public class LoginResult
	{
		public Guid UserId { get; set; }
		public Guid Token { get; set; }
		public bool UserOnboarded { get; set; }
	}
}
