using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;

using SensateIoT.SmartEnergy.Dsmr.Api.Data;
using SensateIoT.SmartEnergy.Dsmr.Api.Middleware;
using SensateIoT.SmartEnergy.Dsmr.Data.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Controllers
{
	[RoutePrefix("dsmr/v1/devices")]
	[EnableCors("*", "*", "*")]
	public class DevicesController : BaseController
	{
		/// <summary>
		/// Get the devices for a user based on a given product token.
		/// </summary>
		/// <returns>List of devices.</returns>
		[HttpGet]
		[Route]
		[ExceptionHandling]
		[SwaggerResponse(HttpStatusCode.OK, "Result response.", typeof(Response<IEnumerable<Device>>))]
		[SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized response.", typeof(Response<object>))]
		public  IHttpActionResult GetDevices()
		{
			var response = new Response<object>();

			if(!this.Request.Properties.TryGetValue("User", out var obj)) { 
				response.AddError("User not found.");
				this.throwUnauthorized(response);
			}

			var user = obj as User;

			if(user == null) {
				response.AddError("User not found/invalid.");
				this.throwUnauthorized(response);
			}

			response.Data = user?.Devices;

			return this.Ok(response);
		}

		private void throwUnauthorized(Response<object> response)
		{
			var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) {
				Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json"),
				ReasonPhrase = "Device not authorized for user"
			};

			throw new HttpResponseException(msg);
		}
	}
}
