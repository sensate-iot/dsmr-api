using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

using Swashbuckle.Swagger.Annotations;

using SensateIoT.SmartEnergy.Dsmr.Api.Attributes;
using SensateIoT.SmartEnergy.Dsmr.Api.Data;
using SensateIoT.SmartEnergy.Dsmr.Api.Exceptions;
using SensateIoT.SmartEnergy.Dsmr.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.Data.Models;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

using DataPoint = SensateIoT.SmartEnergy.Dsmr.Data.DTO.DataPoint;
using EnergyDataPoint = SensateIoT.SmartEnergy.Dsmr.Data.DTO.EnergyDataPoint;
using EnergyHourlyAggregate = SensateIoT.SmartEnergy.Dsmr.Data.DTO.EnergyHourlyAggregate;
using EnvironmentDataPoint = SensateIoT.SmartEnergy.Dsmr.Data.DTO.EnvironmentDataPoint;
using GroupedPowerData = SensateIoT.SmartEnergy.Dsmr.Data.DTO.GroupedPowerData;
using WeeklyHigh = SensateIoT.SmartEnergy.Dsmr.Data.DTO.WeeklyHigh;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Controllers
{
	[RoutePrefix("dsmr/v1/aggregates")]
	[EnableCors("*", "*", "*")]
    public class AggregatesController : BaseController 
    {
	    private readonly IOlapRepository m_olap;

	    public AggregatesController(IOlapRepository olapRepo)
	    {
		    this.m_olap = olapRepo;
	    }

		/// <summary>
		/// Get power data aggregates.
		/// </summary>
		/// <param name="sensorId">DSMR meter ID.</param>
		/// <param name="start">Starting timestamp.</param>
		/// <param name="end">Ending timestmap.</param>
		/// <param name="granularity">Data granularity.</param>
		/// <returns>Sequence of power data.</returns>
        [HttpGet]
		[ExceptionHandling, ProductTokenAuthentication]
		[Route("power/{sensorId}")]
		[SwaggerResponse(HttpStatusCode.OK, "Result response.", typeof(Response<IEnumerable<EnergyDataPoint>>))]
		[SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized response.", typeof(Response<object>))]
        public async Task<IHttpActionResult> GetPowerAggregatesAsync(int sensorId, DateTime? start = null, DateTime? end = null, Granularity granularity = Granularity.Hour)
        {
			this.ThrowIfDeviceUnauthorized(sensorId);
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

		/// <summary>
		/// Get environment data aggregates.
		/// </summary>
		/// <param name="sensorId">DSMR meter ID.</param>
		/// <param name="start">Starting timestamp.</param>
		/// <param name="end">Ending timestmap.</param>
		/// <param name="granularity">Data granularity.</param>
		/// <returns>Sequence of environment data.</returns>
        [HttpGet]
		[ExceptionHandling, ProductTokenAuthentication]
		[Route("environment/{sensorId}")]
		[SwaggerResponse(HttpStatusCode.OK, "Result response.", typeof(Response<IEnumerable<EnvironmentDataPoint>>))]
		[SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized response.", typeof(Response<object>))]
        public async Task<IHttpActionResult> GetEnvironmentAggregatesAsync(int sensorId, DateTime? start = null, DateTime? end = null, Granularity granularity = Granularity.Hour)
        {
			this.ThrowIfDeviceUnauthorized(sensorId);
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

		/// <summary>
		/// Get power data grouped by hour.
		/// </summary>
		/// <param name="sensorId">DSMR meter ID.</param>
		/// <param name="start">Starting timestamp.</param>
		/// <param name="end">Ending timestmap.</param>
		/// <returns>Power data between <paramref name="start"/> and <paramref name="end"/>.</returns>
		[HttpGet, Route("power/{sensorId}/hour")]
		[ProductTokenAuthentication, ExceptionHandling]
		[SwaggerResponse(HttpStatusCode.OK, "Result response.", typeof(Response<IEnumerable<GroupedPowerData>>))]
		[SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized response.", typeof(Response<object>))]
		public async Task<IHttpActionResult> GetPowerDataByHour(int sensorId, DateTime? start = null, DateTime? end = null)
		{
			this.ThrowIfDeviceUnauthorized(sensorId);
			var now = DateTime.UtcNow;

	        if(start == null) {
		        start = now.AddDays(-7);
	        }

	        if(end == null) {
		        end = now.AddDays(1);
	        }

		    start = start.Value.ToUniversalTime();
		    end = end.Value.ToUniversalTime();
			var response = new Response<IEnumerable<GroupedPowerData>>();
			var data = await this.m_olap.LookupPowerDataByHour(sensorId, start.Value, end.Value, CancellationToken.None)
				.ConfigureAwait(false);

			response.Data = data;
			return this.Ok(response);
		}

		/// <summary>
		/// Get energy usage per month.
		/// </summary>
		/// <param name="sensorId">DSMR meter ID.</param>
		/// <param name="start">Starting timestamp.</param>
		/// <param name="end">Ending timestmap.</param>
		/// <returns>Power data between <paramref name="start"/> and <paramref name="end"/>.</returns>
		[HttpGet, Route("energy/{sensorId}")]
		[ProductTokenAuthentication, ExceptionHandling]
		[SwaggerResponse(HttpStatusCode.OK, "Result response.", typeof(Response<EnergyUsageData>))]
		[SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized response.", typeof(Response<object>))]
		public async Task<IHttpActionResult> GetEnergyUsagePerMonth(int sensorId, DateTime? start = null, DateTime? end = null)
		{
			this.ThrowIfDeviceUnauthorized(sensorId);
			var now = DateTime.UtcNow;

	        if(start == null) {
		        start = now.AddDays(-7);
	        }

	        if(end == null) {
		        end = now.AddDays(1);
	        }

		    start = start.Value.ToUniversalTime();
		    end = end.Value.ToUniversalTime();

			var response = new Response<EnergyUsageData>();
			var data = await this.m_olap.LookupEnergyDataAsync(sensorId, start.Value, end.Value, CancellationToken.None)
				.ConfigureAwait(false);

			response.Data = data;
			return this.Ok(response);
		}

		/// <summary>
		/// Get the average energy usage over the given period for a given sensor.
		/// </summary>
		/// <param name="sensorId">Sensor to compute averages for.</param>
		/// <param name="start">Start of period.</param>
		/// <param name="end">End of period.</param>
		/// <returns>List of average energy consumption between <paramref name="start"/> and <paramref name="end"/>.</returns>
	    [HttpGet]
		[Route("energy/{sensorId}/average")]
		[ProductTokenAuthentication, ExceptionHandling]
		[SwaggerResponse(HttpStatusCode.OK, "Result response.", typeof(Response<IEnumerable<EnergyHourlyAggregate>>))]
		[SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized response.", typeof(Response<object>))]
		public async Task<IHttpActionResult> GetHourlyEnergyAverages(int sensorId, DateTime? start = null, DateTime? end = null)
		{
			this.ThrowIfDeviceUnauthorized(sensorId);

			if(start == null) {
				start = DateTime.UtcNow.AddMonths(-1);
			}

			if(end == null) {
				end = DateTime.UtcNow;
			}

			var response = new Response<IEnumerable<EnergyHourlyAggregate>>();
			var data = await this.m_olap.LookupHourlyEnergyAggregates(sensorId, start.Value, end.Value, CancellationToken.None)
				.ConfigureAwait(false);

			response.Data = data;
			return this.Ok(response);
		}

		/// <summary>
		/// Get the highest measurements in the last week.
		/// </summary>
		/// <param name="sensorId">DSMR meter ID.</param>
		/// <returns>The weekly high data point.</returns>
        [HttpGet]
		[ExceptionHandling, ProductTokenAuthentication]
        [Route("weekly-high/{sensorId}")]
		[SwaggerResponse(HttpStatusCode.OK, "Result response.", typeof(Response<IEnumerable<WeeklyHigh>>))]
		[SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized response.", typeof(Response<object>))]
        public async Task<IHttpActionResult> GetWeeklyHighAsync(int sensorId)
        {
	        this.ThrowIfDeviceUnauthorized(sensorId);

			var response = new Response<WeeklyHigh>();
			var data = await this.m_olap.LookupWeeklyHighAsync(sensorId, CancellationToken.None).ConfigureAwait(false);

			response.Data = data;
			return this.Ok(response);
        }

		/// <summary>
		/// Get the last data point.
		/// </summary>
		/// <param name="sensorId">DSMR meter ID.</param>
		/// <returns>Returns the last processed data point.</returns>
        [HttpGet]
		[ExceptionHandling, ProductTokenAuthentication]
        [Route("latest/{sensorId}")]
		[SwaggerResponse(HttpStatusCode.OK, "Result response.", typeof(Response<IEnumerable<DataPoint>>))]
		[SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized response.", typeof(Response<object>))]
        public async Task<IHttpActionResult> LatestAsync(int sensorId)
        {
			this.ThrowIfDeviceUnauthorized(sensorId);

			var response = new Response<DataPoint> {
				Data = await this.m_olap.LookupLastDataPointAsync(sensorId, CancellationToken.None)
					.ConfigureAwait(false)
			};

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
