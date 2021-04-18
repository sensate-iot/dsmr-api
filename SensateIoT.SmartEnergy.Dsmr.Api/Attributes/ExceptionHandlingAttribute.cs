using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Filters;

using log4net;
using Newtonsoft.Json;

using SensateIoT.SmartEnergy.Dsmr.Api.Data;
using SensateIoT.SmartEnergy.Dsmr.Api.Exceptions;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Attributes
{
	public class ExceptionHandlingAttribute : ExceptionFilterAttribute
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(ExceptionHandlingAttribute));

		public override void OnException(HttpActionExecutedContext context)
		{
			switch(context.Exception) {
			case HttpResponseException ex:
				throw ex;
			case InvalidQueryArgumentException _:
				logger.Warn("Unable to complete request due to invalid query arguments.", context.Exception);
				this.respondWithError("Unable to complete request with invalid query arguments.", context.Exception.Message, HttpStatusCode.BadRequest);
				break;
			default:
				logger.Error("Unable to complete request!", context.Exception);
				this.respondWithError("Unable to complete request.", null, HttpStatusCode.InternalServerError);
				break;
			}
		}

		private void respondWithError(string error, string message, HttpStatusCode code)
		{
			var response = new Response<string>();

			if(!string.IsNullOrEmpty(message)) {
				response.AddError(message);
			}

			response.AddError(error);
			var resp = JsonConvert.SerializeObject(response);

			throw new HttpResponseException(new HttpResponseMessage {
				Content = new StringContent(resp, Encoding.UTF8, "application/json"),
				ReasonPhrase = "Unable to complete request",
				StatusCode = code
			});
		}

	}
}
