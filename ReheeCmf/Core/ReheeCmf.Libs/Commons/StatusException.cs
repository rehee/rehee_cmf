using ReheeCmf.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons
{
  public class StatusException : Exception
  {
    public HttpStatusCode StatusCode { get; private set; }
    public string? ErrorCode { get; set; }
    public IEnumerable<ValidationResult>? ValidationError { get; private set; }
    public object? OtherError { get; private set; }
    public int StatusCodeInt => (int)StatusCode;
    public StatusException(string? message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, Exception? innerException = null) : base(message, innerException)
    {
      this.StatusCode = statusCode;
    }
    public StatusException(string? message, string? errorCode, HttpStatusCode statusCode = HttpStatusCode.BadRequest, Exception? innerException = null)
      : this(message, statusCode, innerException)
    {
      this.StatusCode = statusCode;
      this.ErrorCode = errorCode;
    }
    public static void Throw(HttpStatusCode statusCode = HttpStatusCode.BadRequest, string message = "", string errorCode = "", Exception innerException = null)
    {
      throw new StatusException(message, errorCode, statusCode, innerException);
    }
    public static void Throw(IContentResponse response)
    {
      var e = new StatusException(response.ErrorMessage, response.ErrorCode, response.Status);
      e.OtherError = response.ErrorObject;
      e.ValidationError = response.Validation;
      throw e;
    }
    public static void Throw(params ValidationResult[] validationResults)
    {
      var e = new StatusException(Common.ErrorCode_Validation, Common.ErrorCode_Validation, HttpStatusCode.BadRequest);
      e.ValidationError = validationResults;
      throw e;
    }
    public static void ThrowOtherError(object error)
    {
      var e = new StatusException("", "");
      e.OtherError = error;
      throw e;
    }
  }
}
