using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Helper
{
  public static class StatusExceptionHelper
  {
    public static Exception ThrowStatusException(this Exception ex)
    {
      if (ex == null)
      {
        return new Exception();
      }
      if (ex is StatusException statusEx)
      {
        return statusEx;
      }
      return new StatusException(ex.Message, innerException: ex.InnerException);
    }
    public static Exception? Throw(this Exception ex)
    {
      return ex.ThrowStatusException();
    }
  }
}
