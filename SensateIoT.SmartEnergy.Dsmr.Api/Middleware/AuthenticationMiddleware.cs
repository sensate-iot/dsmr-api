using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using log4net;
using Newtonsoft.Json;

using SensateIoT.SmartEnergy.Dsmr.Api.Data;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Middleware
{
	public class AuthenticationMiddleware : DelegatingHandler
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(AuthenticationMiddleware));
		private static readonly Guid staticToken = Guid.Parse("2460ecea-78be-4e87-b913-c713a43184ef");

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			logger.Info("Verifying product token.");

			if(!request.Headers.TryGetValues("X-ProductToken", out var values)) {
				logger.Info("No product token found!");
				return this.respondWithError("Product token missing.", HttpStatusCode.Unauthorized);
			}

			var token = values.First();
			logger.Debug("Found product token: " + token);

			if(!this.verifyToken(token)) {
				logger.Info("Product token invalid. Stopping.");
				return this.respondWithError("Unable to authenticate using product token.", HttpStatusCode.Unauthorized);
			}

			return await base.SendAsync(request, cancellationToken);
		}

		private HttpResponseMessage respondWithError(string error, HttpStatusCode code)
		{
			var response = new Response<string>();

			response.AddError(error);
			var resp = JsonConvert.SerializeObject(response);

			return new HttpResponseMessage(code) {
				Content = new StringContent(resp, Encoding.UTF8, "application/json"),
				ReasonPhrase = "Invalid product token"
			};
		}

		private bool verifyToken(string token)
		{
			if(!Guid.TryParse(token, out var uuid)) {
				return false;
			}

			return uuid == staticToken;
		}
	}
}

