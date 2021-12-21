using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Data.Models;
using SensateIoT.SmartEnergy.Dsmr.Data.Settings;

using User = SensateIoT.SmartEnergy.Dsmr.Data.Models.User;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Repositories
{
	[UsedImplicitly]
	public class AuthenticationRepository : AbstractRepository, IAuthenticationRepository 
	{

		private const string Symbols = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
		private const int KeyLength = 6;

		private const string DsmrApi_SelectProductToken = "DsmrApi_GetProductToken";
		private const string DsmrApi_DsmrApi_SelectUserById = "DsmrApi_SelectUserById";
		private const string DsmrApi_Login = "DsmrApi_Login";
		private const string DsmrApi_Logout = "DsmrApi_Logout";
		private const string DsmrApi_ResetOtpToken = "DsmrApi_ResetOtpToken";

		private readonly Random m_random;
		private readonly IDeviceService m_deviceService;

		public AuthenticationRepository(IDeviceService devices, AppSettings settings) : base(new SqlConnection(settings.DsmrProductConnectionString))
		{
			this.m_random = new Random(DateTime.UtcNow.Millisecond * DateTime.UtcNow.Second);
			this.m_deviceService = devices;
		}

		public async Task<ProductToken> GetProductTokenAsync(Guid token, CancellationToken ct)
		{
			return await this.QuerySingleAsync<ProductToken>(DsmrApi_SelectProductToken, ct, "@token", token.ToString("D"))
				.ConfigureAwait(false);
		}

		public async Task<LoginResult> LoginAsync(string email, string otp, CancellationToken ct)
		{
			return await this.QuerySingleAsync<LoginResult>(DsmrApi_Login, ct, "@email", email, "@token", otp)
				.ConfigureAwait(false);
		}

		public async Task<Data.DTO.User> GetUserAsync(Guid id, CancellationToken ct)
		{
			var user = await this.QuerySingleAsync<User>(DsmrApi_DsmrApi_SelectUserById, ct, "@userId", id.ToString("D"))
				.ConfigureAwait(false);
			var devices = await this.m_deviceService.GetDevicesByUserIdAsync(user.Id, ct).ConfigureAwait(false);

			return new Data.DTO.User {
				Id = user.Id,
				Timestamp = user.Timestamp,
				Email = user.Email,
				Msisdn = $"+{user.Msisdn}",
				Enabled = user.Enabled,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Devices = devices
			};
		}

		public async Task LogoutAsync(string email, CancellationToken ct)
		{
			this.checkToken(ct);
			await this.ExecuteAsync(DsmrApi_Logout, "@email", email).ConfigureAwait(false);
		}

		public async Task<OtpToken> CreateOtpTokenAsync(string email, CancellationToken ct)
		{
			this.checkToken(ct);
			var next = this.nextStringWithSymbols(KeyLength);

			var token = await this.QuerySingleAsync<OtpToken>(DsmrApi_ResetOtpToken, ct, 
			                                                  "@email", email,
			                                                  "@token", next)
				.ConfigureAwait(false);

			return token;
		}

		private void checkToken(CancellationToken ct)
		{
			if(ct.IsCancellationRequested) {
				throw new OperationCanceledException("Logout query cancelled.", ct);
			}
		}

		private string nextStringWithSymbols(int length)
		{
			char[] ary;

			ary = Enumerable.Repeat(Symbols, length)
				.Select(s => s[this.m_random.Next(0, Symbols.Length)]).ToArray();
			return new string(ary);
		}
	}
}
