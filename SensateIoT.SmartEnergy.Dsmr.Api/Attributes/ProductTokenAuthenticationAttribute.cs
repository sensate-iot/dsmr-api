using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using log4net;
using Newtonsoft.Json;

using SensateIoT.SmartEnergy.Dsmr.Api.Data;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Attributes
{
	public class ProductTokenAuthenticationAttribute : AuthorizationFilterAttribute 
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(ProductTokenAuthenticationAttribute));

		private readonly IAuthenticationRepository m_repo;

		public ProductTokenAuthenticationAttribute()
		{
	        this.m_repo = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IAuthenticationRepository)) as IAuthenticationRepository;
		}

		public override async Task OnAuthorizationAsync(HttpActionContext ctx, CancellationToken ct)
		{
			var request = ctx.Request;
			logger.Info("Verifying product token.");

			if(request.Method == HttpMethod.Options) {
				return;
			}

			if(!request.Headers.TryGetValues("X-ProductToken", out var values)) {
				logger.Info("No product token found!");
				this.respondWithError("Product token missing.", null, HttpStatusCode.Forbidden);
			}

			var token = values.First();

			logger.Debug("Found product token: " + token);
			var verify = await this.verifyToken(request, token, ct).ConfigureAwait(false);

			if(!verify) {
				logger.Info("Product token invalid. Stopping.");
				this.respondWithError("Unable to authenticate using product token.", "Product token invalid.", HttpStatusCode.Forbidden);
			}
		}

		private void respondWithError(string error, string message, HttpStatusCode code)
		{
			var response = new Response<string>();

			if(!string.IsNullOrEmpty(message)) {
				response.AddError(message);
			}

			response.AddError(error);
			var resp = JsonConvert.SerializeObject(response);

			throw new HttpResponseException(new HttpResponseMessage {
				Content = new StringContent(resp, Encoding.UTF8, "application/json"),
				ReasonPhrase = "Unable to complete request",
				StatusCode = code
			});
		}

		private async Task<bool> verifyToken(HttpRequestMessage message, string token, CancellationToken ct)
		{
			if(!Guid.TryParse(token, out var uuid)) {
				return false;
			}

			var pt = await this.m_repo.GetProductTokenAsync(uuid, ct)
				.ConfigureAwait(false);

			if(uuid != pt?.Token) {
				return false;
			}

			var user = await this.m_repo.GetUserAsync(pt.UserId, ct).ConfigureAwait(false);
			message.Properties.Add("User", user);

			return true;
		}
	}
}
