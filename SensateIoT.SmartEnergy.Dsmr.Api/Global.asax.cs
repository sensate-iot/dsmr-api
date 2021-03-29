using System.Web.Http;
using System.Web.Mvc;

using Dapper;

using SensateIoT.SmartEnergy.Dsmr.DataAccess.Converters;

namespace SensateIoT.SmartEnergy.Dsmr.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
			SqlMapper.AddTypeHandler(new DateTimeHandler());

            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}
