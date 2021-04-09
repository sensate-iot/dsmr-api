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
	        var auth = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IAuthenticationRepository)) as IAuthenticationRepository;
			//var auth = scope.GetService(typeof(IAuthenticationRepository)) as IAuthenticationRepository;

			config.MapHttpAttributeRoutes();
			config.MessageHandlers.Add(new RequestLoggingMiddleware());
			config.MessageHandlers.Add(new AuthenticationMiddleware(auth));

			config.Formatters.JsonFormatter.SupportedMediaTypes
				.Add(new MediaTypeHeaderValue("text/html"));

	        /*config.Routes.MapHttpRoute(
	            name: "DefaultApi",
	            routeTemplate: "dsmr/{controller}/{id}",
	            defaults: new { id = RouteParameter.Optional }
	        );*/
        }
    }
}
