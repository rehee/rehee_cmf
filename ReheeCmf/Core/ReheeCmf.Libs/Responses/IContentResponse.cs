using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Responses
{
  public interface IContentResponse : ISetValidation, IDisposable
  {
    bool Success { get; set; }
    HttpStatusCode Status { get; set; }
    int IntStatus { get; }
    string? ErrorMessage { get; set; }
    string? ErrorCode { get; set; }
    bool Cancel { get; set; }
    object? ErrorObject { get; set; }
    IEnumerable<ValidationResult>? Validation { get; }
    IEnumerable<ValidationResult>? ExceptionValidation { get; set; }
    object? ContentObject { get; set; }
  }
  public interface IContentResponse<T> : IContentResponse
  {
    T? Content { get; set; }
  }
}
