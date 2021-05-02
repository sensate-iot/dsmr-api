using System.Threading.Tasks;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Adapters
{
	public interface ISmsAdapter
	{
		Task SendAsync(string id, string to, string body, bool retry = true);
	}
}
