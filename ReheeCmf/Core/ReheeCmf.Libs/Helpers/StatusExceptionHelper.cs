﻿namespace ReheeCmf.Helpers
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
    public static Exception? Throw(this Exception? ex)
    {
      return ex.ThrowStatusException();
    }
    public static StatusException GetStatusException(this Exception ex)
    {
      if (ex == null)
      {
        return null;
      }
      if (ex is StatusException exStatus)
      {
        return exStatus;
      }
      if (ex.InnerException != null)
      {
        return StatusExceptionHelper.GetStatusException(ex.InnerException);
      }
      return new StatusException(ex.Message, innerException: ex.InnerException);
    }
  }
}
