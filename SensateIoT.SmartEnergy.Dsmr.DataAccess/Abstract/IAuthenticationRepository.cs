using System;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Data.Models;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract
{
	public interface IAuthenticationRepository : IDisposable
	{
		Task<ProductToken> GetProductTokenAsync(Guid token, CancellationToken ct);
	}
}
