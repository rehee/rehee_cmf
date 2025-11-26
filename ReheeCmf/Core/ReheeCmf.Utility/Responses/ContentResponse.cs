using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Responses
{
  public class ContentResponse<T> : IContentResponse<T>
  {
    public T? Content { get; set; }
    public object? ContentObject
    {
      get
      {
        return Content;
      }
      set
      {
        if (value is T tValue)
        {
          Content = tValue;
        }
      }
    }

    public bool Success { get; set; }
    public HttpStatusCode Status { get; set; }
    public int IntStatus => (int)Status;

    public string? ErrorMessage { get; set; }
    public string? ErrorCode { get; set; }
    public bool Cancel { get; set; }
    public object? ErrorObject { get; set; }

    public IEnumerable<ValidationResult>? Validation { get; set; }

    public IEnumerable<ValidationResult>? ExceptionValidation { get; set; }




    public void SetValidation(params ValidationResult[] validations)
    {
      Validation = validations;
    }
    bool IsDispose { get; set; }

    public bool ValidationError => Validation?.Any() == true && ExceptionValidation?.Any() == true;

    public void Dispose()
    {
      if (IsDispose)
      {
        return;
      }
      IsDispose = true;
      Content = default(T);
      Validation = null;
      ExceptionValidation = null;
      ErrorObject = null;
      GC.SuppressFinalize(this);
    }
  }
}
