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
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Middleware
{
	public class AuthenticationMiddleware : DelegatingHandler
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(AuthenticationMiddleware));

		private readonly IAuthenticationRepository m_repo;

		public AuthenticationMiddleware(IAuthenticationRepository repo)
		{
			this.m_repo = repo;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			logger.Info("Verifying product token.");

			if(request.RequestUri.PathAndQuery.Contains("swagger")) {
				return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
			}

			if(!request.Headers.TryGetValues("X-ProductToken", out var values)) {
				logger.Info("No product token found!");
				return this.respondWithError("Product token missing.", HttpStatusCode.Unauthorized);
			}

			var token = values.First();
			logger.Debug("Found product token: " + token);
			var verify = await this.verifyToken(request, token, cancellationToken).ConfigureAwait(false);

			if(!verify) {
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

		private async Task<bool> verifyToken(HttpRequestMessage message, string token, CancellationToken ct)
		{
			if(!Guid.TryParse(token, out var uuid)) {
				return false;
			}

			var pt = await this.m_repo.GetProductTokenAsync(uuid, ct).ConfigureAwait(false);

			if(uuid != pt.Token) {
				return false;
			}

			var user = await this.m_repo.GetUserAsync(pt.UserId, ct).ConfigureAwait(false);
			message.Properties.Add("User", user);

			return true;
		}
	}
}

