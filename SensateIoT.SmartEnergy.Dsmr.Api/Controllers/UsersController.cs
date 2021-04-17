using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

using SensateIoT.SmartEnergy.Dsmr.Api.Data;
using SensateIoT.SmartEnergy.Dsmr.Api.Middleware;
using SensateIoT.SmartEnergy.Dsmr.Data.DTO;

using Swashbuckle.Swagger.Annotations;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Controllers
{
	[RoutePrefix("dsmr/v1/users")]
	[EnableCors("*", "*", "*")]
	public class UsersController : BaseController
	{
		[HttpGet]
		[Route]
		[ExceptionHandling]
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
	}
}