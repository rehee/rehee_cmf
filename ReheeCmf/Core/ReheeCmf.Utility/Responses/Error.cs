using System.Net;
using System.Runtime.Serialization;

namespace ReheeCmf.Responses
{
	[DataContract]
	public class Error
	{
		public static Error? New(Exception ex)
		{
			if (ex == null)
			{
				return null;
			}
			StatusException? statusException = null;
			if (ex is StatusException ex2)
			{
				statusException = ex2;
			}
			var error = new Error(
				statusException?.Message ?? ex.Message,
				statusException?.ErrorCode ?? "",
				statusException?.StatusCode ?? HttpStatusCode.InternalServerError,
				New(ex.InnerException));
			error.ValidationError = statusException?.ValidationError;
			error.OtherError = statusException?.OtherError;
			return error;
		}
		public Error(string message, string errorCode, HttpStatusCode status, Error? innerError)
		{
			Message = message;
			ErrorCode = errorCode;
			Status = status;
			InnerError = innerError;
		}
		[DataMember(EmitDefaultValue = false)]
		public string Message { get; private set; }
		[DataMember(EmitDefaultValue = false)]
		public string ErrorCode { get; private set; }
		[DataMember(EmitDefaultValue = false)]
		public HttpStatusCode Status { get; private set; }
		[DataMember(EmitDefaultValue = false)]
		public Error? InnerError { get; private set; }
		[DataMember(EmitDefaultValue = false)]
		public IEnumerable<ValidationResult>? ValidationError { get; private set; }
		[DataMember(EmitDefaultValue = false)]
		public object? OtherError { get; set; }
	}
}
