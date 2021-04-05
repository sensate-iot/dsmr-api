using System;

namespace SensateIoT.SmartEnergy.Dsmr.Api.Exceptions
{
	public class InvalidQueryArgumentException : ArgumentException 
	{
		public InvalidQueryArgumentException(string parameterName, string message) : base(message, parameterName)
		{
		}
	}
}
