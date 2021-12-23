using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Services
{
	public class DeviceService : IDeviceService 
	{
		private readonly IDeviceRepository _deviceRepository;
		private readonly IOlapRepository _olapRepository;

		public DeviceService(IDeviceRepository devRepo, IOlapRepository olap)
		{
			this._deviceRepository = devRepo;
			this._olapRepository = olap;
		}

		public async Task<IEnumerable<Device>> GetDevicesByUserIdAsync(Guid userId, CancellationToken ct)
		{
			var tmp = await this._deviceRepository.GetDevicesByUserIdAsync(userId, ct).ConfigureAwait(false);
			var devices = tmp.ToList();

			foreach(var device in devices) {
				var capabilities = await this._olapRepository.LookupDeviceCapabilities(device.Id, ct).ConfigureAwait(false);

				device.HasSolarCells = capabilities.HasSolarCells;
				device.HasGasSensor = capabilities.HasGasMeter;
			}

			return devices;
		}
	}
}