using System.Threading.Tasks;

using log4net;
using SensateIoT.SmartEnergy.Dsmr.Data.Settings;

using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Adapters
{
	public class TwilioAdapter : ISmsAdapter
	{
		private const int AlphaNumericNotSupportedCode = 21612;
		private static ILog logger = LogManager.GetLogger(nameof(TwilioAdapter));

		private readonly IncomingPhoneNumberResource m_resouce;

		public TwilioAdapter(AppSettings settings)
		{
			this.m_resouce = IncomingPhoneNumberResource.Fetch(settings.TwilioPhoneSid);
		}

		public static void Init(string account, string auth)
		{
			TwilioClient.Init(account, auth);
		}

		public async Task SendAsync(string id, string to, string body, bool retry = true)
		{
			await Task.Run(() => { this.Send(id, to, body); });
		}

		private void Send(string id, string to, string body, bool retry = true)
		{
			try {
				MessageResource.Create(
					new PhoneNumber(to),
					from: new PhoneNumber(id),
					body: body
				);
			} catch(ApiException ex) {
				if(ex.Code == AlphaNumericNotSupportedCode && retry) {
					logger.Warn("Unable to send message using alpha-numeric ID. Trying with phone number..");
					this.Send(this.m_resouce.PhoneNumber.ToString(), to, body, false);
				} else {
					logger.Error($"Unable to send text message: {ex.Message}");
				}
			}
		}
	}
}