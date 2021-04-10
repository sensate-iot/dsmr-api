using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using SensateIoT.SmartEnergy.Dsmr.Data.Models;
using SensateIoT.SmartEnergy.Dsmr.Data.Settings;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

using Device = SensateIoT.SmartEnergy.Dsmr.Data.DTO.Device;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Repositories
{
	[UsedImplicitly]
	public class AuthenticationRepository : AbstractRepository, IAuthenticationRepository 
	{
		private const string DsmrApi_SelectProductToken = "DsmrApi_GetProductToken";
		private const string DsmrApi_DsmrApi_SelectUserById = "DsmrApi_SelectUserById";
		private const string DsmrApi_SelectDevicesForUser = "DsmrApi_SelectDevicesForUser";

		public AuthenticationRepository(AppSettings settings) : base(new SqlConnection(settings.DsmrProductConnectionString))
		{
		}

		public async Task<ProductToken> GetProductTokenAsync(Guid token, CancellationToken ct)
		{
			return await this.QuerySingleAsync<ProductToken>(DsmrApi_SelectProductToken, ct, "@token", token.ToString("D"))
				.ConfigureAwait(false);
		}

		public async Task<Data.DTO.User> GetUserAsync(Guid id, CancellationToken ct)
		{
			var user = await this.QuerySingleAsync<User>(DsmrApi_DsmrApi_SelectUserById, ct, "@userId", id.ToString("D"))
				.ConfigureAwait(false);
			var devices = await this.QueryAsync<Data.Models.Device>(DsmrApi_SelectDevicesForUser, ct, "@userId", id.ToString("D"))
				.ConfigureAwait(false);

			return new Data.DTO.User {
				Id = user.Id,
				Timestamp = user.Timestamp,
				Email = user.Email,
				Enabled = user.Enabled,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Devices = devices.Select(x => new Device {
					Timestamp = x.Timestamp,
					Enabled = x.Enabled,
					EnvironmentSensorId = x.EnvironmentSensorId,
					GasSensorId = x.GasSensorId,
					Id = x.Id,
					PowerSensorId = x.PowerSensorId,
					ServiceName = x.ServiceName
				})
			};
		}
	}
}
