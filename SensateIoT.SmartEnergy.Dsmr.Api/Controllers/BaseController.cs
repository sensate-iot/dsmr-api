using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using SensateIoT.SmartEnergy.Dsmr.Api.Data;
using SensateIoT.SmartEnergy.Dsmr.Data.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Controllers
{
	public abstract class BaseController : ApiController
	{
		protected bool AuthorizeDeviceForUser(int deviceId, out string error)
		{
			error = null;

			if(!this.Request.Properties.TryGetValue("User", out var obj)) {
				error = "User not found.";
			}

			var user = obj as User;

			foreach(var device in user.Devices) {
				if(this.verifyDevice(device, deviceId, out error)) {
					return true;
				}
			}

			return false;
		}

		protected void ThrowIfDeviceUnauthorized(int sensorId)
		{
	        var response = new Response<object>();

	        if(this.AuthorizeDeviceForUser(sensorId, out var error)) {
		        return;
	        }

	        response.AddError(error);
	        response.AddError($"Device {sensorId} not authorized for the current user.");

	        var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) {
		        Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8,
		                                    "application/json"),
		        ReasonPhrase = "Device not authorized for user"
	        };

	        throw new HttpResponseException(msg);
		}

		private bool verifyDevice(Device device, int deviceId, out string error)
		{
			if(device.Id != deviceId) {
				error = null;
				return false;
			}

			if(!device.Enabled) {
				error = "Device not enabled.";
				return false;
			}

			error = null;
			return true;
		}
	}
}
