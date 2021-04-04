using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

using SensateIoT.SmartEnergy.Dsmr.Api.Data;
using SensateIoT.SmartEnergy.Dsmr.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Data.Models;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

using EnergyDataPoint = SensateIoT.SmartEnergy.Dsmr.Data.DTO.EnergyDataPoint;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Controllers
{
	[RoutePrefix("dsmr/v1/aggregates")]
    public class AggregatesController : ApiController
    {
	    private readonly IOlapRepository m_olap;

	    public AggregatesController(IOlapRepository olapRepo)
	    {
		    this.m_olap = olapRepo;
	    }

        [HttpGet]
		[Route("{sensorId}")]
        public async Task<IHttpActionResult> Index(int sensorId, DateTime? start, DateTime? end = null, Granularity granularity = Granularity.Hour)
        {
	        var now = DateTime.UtcNow;

	        if(start == null) {
				start = now.Date;
	        }

	        if(end == null) {
				end = now.Date.AddDays(1).AddTicks(-1);
	        }

			return this.Ok(new Response<IEnumerable<EnergyDataPoint>> {
				Data = await this.GetEnergyDataAsync(sensorId, start.Value, end.Value, granularity, CancellationToken.None).ConfigureAwait(false)
	        });
        }

        [HttpGet]
        [Route("{sensorId}/latest")]
        public async Task<IHttpActionResult> LatestAsync(int sensorId)
        {
	        return this.Ok(new Response<DataPoint> {
				Data = await this.m_olap.LookupLastDataPointAsync(sensorId, CancellationToken.None).ConfigureAwait(false)
	        });
        }

        private async Task<IEnumerable<EnergyDataPoint>> GetEnergyDataAsync(int id, DateTime start, DateTime end, Granularity g, CancellationToken ct)
        {
	        IEnumerable<EnergyDataPoint> values;

	        switch(g) {
	        case Granularity.Hour:
		        values = await this.m_olap.LookupPowerDataPerHourAsync(id, start, end, ct).ConfigureAwait(false);
		        break;
	        case Granularity.Day:
		        values = await this.m_olap.LookupPowerDataPerDayAsync(id, start, end, ct).ConfigureAwait(false);
		        break;
	        default:
		        throw new ArgumentOutOfRangeException(nameof(g), g, null);
	        }

	        return values;
        }
    }
}
