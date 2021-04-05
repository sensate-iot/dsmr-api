using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Data
{
	public class Response<TValue>
	{
		[JsonProperty("id")]
		public Guid Id { get; }
		[JsonProperty("errors")]
		public ICollection<string> Errors { get; private set; }
		[JsonProperty("data")]
		public TValue Data { get; set; }

		public Response()
		{
			this.Id = Guid.NewGuid();
		}

		public void AddError(string error)
		{
			if(this.Errors == null) {
				this.Errors = new List<string>();
			}

			this.Errors.Add(error);
		}
	}
}
