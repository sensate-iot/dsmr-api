namespace SensateIoT.SmartEnergy.Dsmr.Data.Settings
{
	public class AppSettings
	{
		public string OlapConnectionString { get; set; }
		public string DsmrProductConnectionString { get; set; }
		public string TwilioPhoneSid { get; set; }
		public bool OtpEnabled { get; set; }
		public string SenderId { get; set; }
	}
}
