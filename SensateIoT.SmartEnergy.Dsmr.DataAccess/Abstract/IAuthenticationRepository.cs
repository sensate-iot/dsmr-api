using System;
using System.Threading.Tasks;
using System.Threading;

using SensateIoT.SmartEnergy.Dsmr.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Data.Models;

using User = SensateIoT.SmartEnergy.Dsmr.Data.DTO.User;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract
{
	public interface IAuthenticationRepository : IDisposable
	{
		Task<ProductToken> GetProductTokenAsync(Guid token, CancellationToken ct);
		Task<User> GetUserAsync(Guid id, CancellationToken ct);
		Task<LoginResult> LoginAsync(string email, string otp, CancellationToken ct);
		Task LogoutAsync(string email, CancellationToken ct);
		Task<OtpToken> CreateOtpTokenAsync(string id, CancellationToken ct);
	}
}
