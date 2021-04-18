using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using Microsoft.IO;

using log4net;
using Newtonsoft.Json;

using SensateIoT.SmartEnergy.Dsmr.Api.Data;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Middleware
{
	public class RequestLoggingMiddleware : DelegatingHandler
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(RequestLoggingMiddleware));
		private const int readChunkBufferLength = 4096;

		private readonly RecyclableMemoryStreamManager m_manager;

		public RequestLoggingMiddleware()
		{
			this.m_manager = new RecyclableMemoryStreamManager();
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			logger.Info($"Received request: {request.Method.Method} {request.RequestUri.PathAndQuery} from {getClientIp(request)}");
			var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
			await this.logResponse(response).ConfigureAwait(false);

			return response;
		}

		private static string readStreamInChunks(Stream stream)
		{
			string content;

			stream.Seek(0, SeekOrigin.Begin);

			using(var textWriter = new StringWriter()) {
				using(var reader = new StreamReader(stream)) {
					var readChunk = new char[readChunkBufferLength];
					int readChunkLength;
					do {
						readChunkLength = reader.ReadBlock(readChunk,
						                                   0,
						                                   readChunkBufferLength);
						textWriter.Write(readChunk, 0, readChunkLength);
					} while(readChunkLength > 0);

					content = textWriter.ToString();
				}
			}

			return content;
		}

		private static string getClientIp(HttpRequestMessage message)
		{
			if(!message.Headers.TryGetValues("X-Real-IP", out var realIPs)) {
				return HttpContext.Current.Request.UserHostAddress;
			}

			return realIPs.First();
		}

		private async Task logResponse(HttpResponseMessage response)
		{
			if(response.Content == null) {
				return;
			}

			using(var stream = this.m_manager.GetStream()) {
				await response.Content.CopyToAsync(stream).ConfigureAwait(false);
				var body = readStreamInChunks(stream);

				logger.Info($"Request resulted in {response.StatusCode} with ID: {getResponseId(body):D}");
			}
		}

		private static Guid getResponseId(string body)
		{
			Guid id;

			try {
				var result = JsonConvert.DeserializeObject<Response<object>>(body);

				if(result == null) {
					return Guid.Empty;
				}

				id = result.Id;
			} catch(JsonException ex) {
				logger.Warn($"Response not standardized: {body}.", ex);
				id = Guid.Empty;
			}

			return id;
		}
	}
}
