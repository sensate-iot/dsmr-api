using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

using log4net;
using Swashbuckle.Swagger.Annotations;

using SensateIoT.SmartEnergy.Dsmr.Api.Adapters;
using SensateIoT.SmartEnergy.Dsmr.Api.Attributes;
using SensateIoT.SmartEnergy.Dsmr.Api.Data;
using SensateIoT.SmartEnergy.Dsmr.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Data.Settings;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;


namespace SensateIoT.SmartEnergy.Dsmr.Api.Controllers
{
	[RoutePrefix("dsmr/v1/users")]
	[EnableCors("*", "*", "*")]
	public class UsersController : BaseController
	{
		private static ILog logger = LogManager.GetLogger(nameof(UsersController));

		private readonly IAuthenticationRepository m_repo;
		private readonly ISmsAdapter m_adapter;
		private readonly AppSettings m_settings;

		public UsersController(IAuthenticationRepository repo, ISmsAdapter adapter, AppSettings settings)
		{
			this.m_repo = repo;
			this.m_adapter = adapter;
			this.m_settings = settings;
		}

		/// <summary>
		/// Get the current user.
		/// </summary>
		/// <returns>The current user.</returns>
		[HttpGet]
		[Route]
		[ExceptionHandling, ProductTokenAuthentication]
		[SwaggerResponse(HttpStatusCode.OK, "Result response.", typeof(Response<IEnumerable<User>>))]
		[SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized response.", typeof(Response<object>))]
		public IHttpActionResult GetCurrentUser()
		{
			var response = new Response<User>();
			var user = this.GetUser();

			if(user == null) {
				response.AddError("User not found!");
				this.ThrowResponse(response, HttpStatusCode.NotFound);
			}

			response.Data = user;
			return this.Ok(response);
		}

		/// <summary>
		/// Attempt to login using a email and OTP.
		/// </summary>
		/// <param name="request">Username/password combo.</param>
		/// <returns>Login response.</returns>
		[HttpPost]
		[Route("login")]
		[ExceptionHandling]
		[SwaggerResponse(HttpStatusCode.OK, "Result response.", typeof(Response<IEnumerable<LoginResult>>))]
		[SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized response.", typeof(Response<object>))]
		[SwaggerResponse(HttpStatusCode.Forbidden, "Unauthorized response.", typeof(Response<object>))]
		[SwaggerResponse(HttpStatusCode.BadRequest, "Invalid login request.", typeof(Response<object>))]
		public async Task<IHttpActionResult> Login([FromBody] LoginRequest request)
		{
			var response = new Response<LoginResult>();

			this.verifyLoginRequest(request);
			var result = await this.m_repo.LoginAsync(request.Email, request.Otp, CancellationToken.None)
				.ConfigureAwait(false);

			if(result == null) {
				response.AddError("Unable to login.");
				this.ThrowResponse(response, HttpStatusCode.Unauthorized);
			}

			response.Data = result;

			return this.Ok(response);
		}

		/// <summary>
		/// Request a new OTP.
		/// </summary>
		/// <param name="request">OTP request, containing the username/email.</param>
		/// <returns>OTP request result.</returns>
		[HttpPost, Route("otp")]
		[ExceptionHandling]
		[SwaggerResponse(HttpStatusCode.NoContent, "Successful OTP response")]
		[SwaggerResponse(HttpStatusCode.BadRequest, "Invalid OTP request.", typeof(Response<object>))]
		public async Task<IHttpActionResult> GetOtpToken([FromBody] OtpRequest request)
		{
			var response = new Response<object>();

			if(string.IsNullOrEmpty(request?.Email)) {
				response.AddError("Invalid OTP request. Email property is required.");
				this.ThrowResponse(response, HttpStatusCode.BadRequest);
			}

			var token = await this.m_repo.CreateOtpTokenAsync(request.Email, CancellationToken.None).ConfigureAwait(false);

			if(string.IsNullOrEmpty(token?.Token) || token.Msisdn == 0L) {
				response.AddError($"Unable to send OTP to {request.Email}.");
				this.ThrowResponse(response, HttpStatusCode.BadRequest);
			}

			var msisdn = $"+{token.Msisdn}";
			logger.Info($"Writing OTP token to {msisdn}.");

			await this.m_adapter.SendAsync(this.m_settings.SenderId, msisdn, $"Your OTP code is {token.Token}.")
				.ConfigureAwait(false);

			return this.StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Logout the current user.
		/// </summary>
		/// <param name="request">Logout request.</param>
		/// <returns>Logout request result.</returns>
		[HttpPost]
		[Route("logout")]
		[ExceptionHandling, ProductTokenAuthentication]
		[SwaggerResponse(HttpStatusCode.NoContent, "Successful logout response")]
		[SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized response.", typeof(Response<object>))]
		[SwaggerResponse(HttpStatusCode.Forbidden, "Unauthorized response.", typeof(Response<object>))]
		[SwaggerResponse(HttpStatusCode.BadRequest, "Invalid logout request.", typeof(Response<object>))]
		public async Task<IHttpActionResult> Logout([FromBody] LogoutRequest request)
		{
			var response = new Response<string>();
			logger.Info($"Attempting to logout user {request?.Email}");

			if(string.IsNullOrEmpty(request?.Email)) {
				logger.Info("Invalid login.");

				response.AddError("Unable to logout unknown user.");
				this.ThrowResponse(response, HttpStatusCode.BadRequest);
			}

			var user = this.GetUser();

			if(user.Email == request.Email) {
				logger.Warn($"Invalid login attempted by user {user.Id:D} on {request.Email}");
			}

			await this.m_repo.LogoutAsync(user.Email, CancellationToken.None).ConfigureAwait(false);
			logger.Info($"User {request.Email} has been logged out.");
			return this.StatusCode(HttpStatusCode.NoContent);
		}
		
		private void verifyLoginRequest(LoginRequest request)
		{
			var response = new Response<LoginResult>();

			if(string.IsNullOrEmpty(request?.Otp)) {
				response.AddError("Unable to authenticate: OTP missing.");
			}

			if(string.IsNullOrEmpty(request?.Email)) {
				response.AddError("Unable to authenticate: Email missing.");
			}

			if((response.Errors == null || response.Errors.Count <= 0) && this.ModelState.IsValid) {
				return;
			}

			this.ThrowResponse(response, HttpStatusCode.BadRequest);
		}
	}
}
