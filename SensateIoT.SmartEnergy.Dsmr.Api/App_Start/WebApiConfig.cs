using System.Net.Http.Headers;
using System.Web.Http;

using SensateIoT.SmartEnergy.Dsmr.Api.Middleware;
using SensateIoT.SmartEnergy.Dsmr.DataAccess.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
	        config.EnableCors();
			config.MapHttpAttributeRoutes();
			config.MessageHandlers.Add(new RequestLoggingMiddleware());

			config.Formatters.JsonFormatter.SupportedMediaTypes
				.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
