using System.Net.Http.Headers;
using System.Web.Http;

namespace SensateIoT.SmartEnergy.Dsmr.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SupportedMediaTypes
	            .Add(new MediaTypeHeaderValue("text/html"));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "dsmr/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
