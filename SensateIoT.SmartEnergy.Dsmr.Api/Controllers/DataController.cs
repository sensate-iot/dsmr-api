using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

using Swashbuckle.Swagger.Annotations;

using SensateIoT.SmartEnergy.Dsmr.Api.Data;
using SensateIoT.SmartEnergy.Dsmr.Api.Middleware;
using SensateIoT.SmartEnergy.Dsmr.Data.DTO;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Controllers
{
	[RoutePrefix("dsmr/v1/data")]
	public class DataController : BaseController 
	{
	    private readonly IOlapRepository m_olap;

	    public DataController(IOlapRepository olapRepo)
	    {
		    this.m_olap = olapRepo;
	    }

	    [HttpGet]
		[ExceptionHandling]
	    [Route("{sensorId}")]
		[SwaggerResponse(HttpStatusCode.OK, "Result response.", typeof(Response<IEnumerable<Device>>))]
		[SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized response.", typeof(Response<object>))]
	    public async Task<IHttpActionResult> GetPowerAggregatesAsync(int sensorId, DateTime? start = null, DateTime? end = null)
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

		    var response = new Response<IEnumerable<DataPoint>>();
		    var data = await this.m_olap.LookupDataPointsAsync(sensorId, start.Value, end.Value, CancellationToken.None)
			    .ConfigureAwait(false);

		    response.Data = data;
		    return this.Ok(response);
		}
	}
}
