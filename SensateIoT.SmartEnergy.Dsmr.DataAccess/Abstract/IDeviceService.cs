using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SensateIoT.SmartEnergy.Dsmr.Data.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract
{
	public interface IDeviceService
	{
		Task<IEnumerable<Device>> GetDevicesByUserIdAsync(Guid userId, CancellationToken ct);
	}
}