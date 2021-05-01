using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

using log4net;
using Swashbuckle.Swagger.Annotations;

using SensateIoT.SmartEnergy.Dsmr.Api.Attributes;
using SensateIoT.SmartEnergy.Dsmr.Api.Data;
using SensateIoT.SmartEnergy.Dsmr.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;


namespace SensateIoT.SmartEnergy.Dsmr.Api.Controllers
{

	[RoutePrefix("dsmr/v1/users")]
	[EnableCors("*", "*", "*")]
	public class UsersController : BaseController
	{
		private static ILog logger = LogManager.GetLogger(nameof(UsersController));

		private readonly IAuthenticationRepository m_repo;

		public UsersController(IAuthenticationRepository repo)
		{
			this.m_repo = repo;
		}

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

		[HttpPost]
		[Route("logout")]
		[ExceptionHandling, ProductTokenAuthentication]
		[SwaggerResponse(HttpStatusCode.NoContent, "Successful logout response")]
		[SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized response.", typeof(Response<object>))]
		[SwaggerResponse(HttpStatusCode.Forbidden, "Unauthorized response.", typeof(Response<object>))]
		[SwaggerResponse(HttpStatusCode.BadRequest, "Invalid logout request.", typeof(Response<object>))]
		public async Task<IHttpActionResult> Login([FromBody] LogoutRequest request)
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