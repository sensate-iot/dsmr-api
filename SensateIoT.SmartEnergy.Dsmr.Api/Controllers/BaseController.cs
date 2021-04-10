using System.Web.Http;

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
