using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

using Newtonsoft.Json;

using SensateIoT.SmartEnergy.Dsmr.Api.Data;
using SensateIoT.SmartEnergy.Dsmr.Api.Exceptions;
using SensateIoT.SmartEnergy.Dsmr.Api.Middleware;
using SensateIoT.SmartEnergy.Dsmr.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

using EnergyDataPoint = SensateIoT.SmartEnergy.Dsmr.Data.DTO.EnergyDataPoint;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Controllers
{
	[RoutePrefix("dsmr/v1/aggregates")]
    public class AggregatesController : BaseController 
    {
	    private readonly IOlapRepository m_olap;

	    public AggregatesController(IOlapRepository olapRepo)
	    {
		    this.m_olap = olapRepo;
	    }

        [HttpGet]
		[ExceptionHandling]
		[Route("power/{sensorId}")]
        public async Task<IHttpActionResult> GetPowerAggregatesAsync(int sensorId, DateTime? start = null, DateTime? end = null, Granularity granularity = Granularity.Hour)
        {
	        var now = DateTime.UtcNow;

	        if(start == null) {
				start = now.Date;
	        }

	        if(end == null) {
				end = now.Date.AddDays(1).AddTicks(-1);
	        }

		    start = start.Value.ToUniversalTime();
		    end = end.Value.ToUniversalTime();

			var response = new Response<IEnumerable<EnergyDataPoint>>();
			var data = await this
				.getPowerDataAsync(sensorId, start.Value, end.Value, granularity, CancellationToken.None)
				.ConfigureAwait(false);

			response.Data = data;
			return this.Ok(response);
        }

        [HttpGet]
		[ExceptionHandling]
		[Route("environment/{sensorId}")]
        public async Task<IHttpActionResult> GetEnvironmentAggregatesAsync(int sensorId, DateTime? start = null, DateTime? end = null, Granularity granularity = Granularity.Hour)
        {
	        var now = DateTime.UtcNow;

	        if(start == null) {
				start = now.Date;
	        }

	        if(end == null) {
				end = now.Date.AddDays(1).AddTicks(-1);
	        }

		    start = start.Value.ToUniversalTime();
		    end = end.Value.ToUniversalTime();

			var response = new Response<IEnumerable<EnvironmentDataPoint>>();
			var data = await this
				.getEnvironmentDataAsync(sensorId, start.Value, end.Value, granularity, CancellationToken.None)
				.ConfigureAwait(false);

			response.Data = data;
			return this.Ok(response);
        }

        [HttpGet]
		[ExceptionHandling]
        [Route("weekly-high/{sensorId}")]
        public async Task<IHttpActionResult> GetWeeklyHighAsync(int sensorId)
        {
			var response = new Response<WeeklyHigh>();
			var data = await this.m_olap.LookupWeeklyHighAsync(sensorId, CancellationToken.None).ConfigureAwait(false);

			response.Data = data;
			return this.Ok(response);
        }

        [HttpGet]
		[ExceptionHandling]
        [Route("latest/{sensorId}")]
        public async Task<IHttpActionResult> LatestAsync(int sensorId)
        {
	        var response = new Response<DataPoint>();

			this.ThrowIfDeviceUnauthorized(sensorId);

	        response.Data = await this.m_olap.LookupLastDataPointAsync(sensorId, CancellationToken.None)
		        .ConfigureAwait(false);

	        return this.Ok(response);
        }

        private async Task<IEnumerable<EnvironmentDataPoint>> getEnvironmentDataAsync(int id, DateTime start, DateTime end, Granularity g, CancellationToken ct)
        {
	        IEnumerable<EnvironmentDataPoint> values;

	        switch(g) {
	        case Granularity.Hour:
		        values = await this.m_olap.LookupEnvironmentDataPerHourAsync(id, start, end, ct).ConfigureAwait(false);
		        break;
	        case Granularity.Day:
		        values = await this.m_olap.LookupEnvironmentDataPerDayAsync(id, start, end, ct).ConfigureAwait(false);
		        break;
	        default:
				throw new InvalidQueryArgumentException("granularity", "Supported granularity: hour, day");
	        }

	        return values;
        }

        private async Task<IEnumerable<EnergyDataPoint>> getPowerDataAsync(int id, DateTime start, DateTime end, Granularity g, CancellationToken ct)
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
				throw new InvalidQueryArgumentException("granularity", "Supported granularity: hour, day");
	        }

	        return values;
        }
    }
}
