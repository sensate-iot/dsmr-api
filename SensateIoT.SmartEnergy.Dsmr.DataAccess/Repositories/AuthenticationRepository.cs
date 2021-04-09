using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Data.Models;
using SensateIoT.SmartEnergy.Dsmr.Data.Settings;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Repositories
{
	public class AuthenticationRepository : AbstractRepository, IAuthenticationRepository 
	{
		private const string DsmrApi_SelectProductToken = "DsmrApi_GetProductToken";

		public AuthenticationRepository(AppSettings settings) : base(new SqlConnection(settings.DsmrProductConnectionString))
		{
		}

		public async Task<ProductToken> GetProductTokenAsync(Guid token, CancellationToken ct)
		{
			return await this.QuerySingleAsync<ProductToken>(DsmrApi_SelectProductToken, "@token", token.ToString("D"))
				.ConfigureAwait(false);
		}
	}
}
