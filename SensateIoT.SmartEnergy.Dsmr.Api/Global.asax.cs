using System.Web.Http;
using System.Web.Mvc;

using Dapper;
using log4net;

using SensateIoT.SmartEnergy.Dsmr.DataAccess.Converters;

namespace SensateIoT.SmartEnergy.Dsmr.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
	    private static readonly ILog logger = LogManager.GetLogger(nameof(WebApiApplication));

        protected void Application_Start()
        {
			logger.Info("Starting DMSR Web API.");
			SqlMapper.AddTypeHandler(new DateTimeHandler());

            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}
