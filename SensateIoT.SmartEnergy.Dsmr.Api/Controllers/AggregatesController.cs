using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Controllers
{
    public class AggregatesController : ApiController
    {
	    private readonly IOlapRepository m_olap;

	    public AggregatesController(IOlapRepository olapRepo)
	    {
		    this.m_olap = olapRepo;
	    }

        [HttpGet]
        public async Task<IHttpActionResult> Index()
        {
	        var now = DateTime.UtcNow;
	        var startDate = now.Date;
	        var endDate = now.Date.AddDays(1).AddTicks(-1);

			return this.Ok(new {
		        Status = "OK",
		        Data = await this.m_olap
			        .LookupEnergyDataPerHour(1, startDate, endDate, CancellationToken.None)
			        .ConfigureAwait(false)
	        });
        }
    }
}
