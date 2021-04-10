using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using SensateIoT.SmartEnergy.Dsmr.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Data.Settings;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Repositories
{
	[UsedImplicitly]
	public class DeviceRepository : AbstractRepository, IDeviceRepository
	{
		private const string DsmrApi_SelectDevicesForUser = "DsmrApi_SelectDevicesForUser";

		public DeviceRepository(AppSettings settings) : base(new SqlConnection(settings.DsmrProductConnectionString))
		{
		}

		public async Task<IEnumerable<Device>> GetDevicesByUserIdAsync(Guid id, CancellationToken ct)
		{
			var devices = await this.QueryAsync<Data.Models.Device>(DsmrApi_SelectDevicesForUser, ct, "@userId", id.ToString("D"))
				.ConfigureAwait(false);

			return devices.Select(x => new Device {
				Timestamp = x.Timestamp,
				Enabled = x.Enabled,
				EnvironmentSensorId = x.EnvironmentSensorId,
				GasSensorId = x.GasSensorId,
				Id = x.Id,
				PowerSensorId = x.PowerSensorId,
				ServiceName = x.ServiceName
			});
		}
	}
}
