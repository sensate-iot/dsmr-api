using System;
using System.Collections.Generic;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Data
{
	public class Response<TValue>
	{
		public Guid Id { get; }
		public ICollection<string> Errors { get; private set; }
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
