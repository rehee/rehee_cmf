﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReheeCmf.Responses;

namespace ReheeCmf.Servers.Filters
{
  public class CmfExceptionFilter : IExceptionFilter
  {
    private readonly IContextScope<Tenant> detail;
    public void OnException(ExceptionContext context)
    {

      if (!context.ExceptionHandled)
      {
        var exS = CmfExceptionFilter.GetStatusException(context.Exception);
        if (exS != null)
        {
          if (exS.ErrorCode == Common.ErrorCode_Validation)
          {
            context.Result = new ObjectResult(exS.ValidationError)
            {
              StatusCode = exS?.StatusCodeInt ?? 500
            };
            context.ExceptionHandled = true;
            return;
          }
        }
        context.Result = new ObjectResult(Error.New(exS ?? context.Exception))
        {
          StatusCode = exS?.StatusCodeInt ?? 500
        };


        context.ExceptionHandled = true;
      }
    }

    public static StatusException GetStatusException(Exception ex)
    {
      if (ex == null)
      {
        return null;
      }
      if (ex is StatusException exStatus)
      {
        return exStatus;
      }
      return CmfExceptionFilter.GetStatusException(ex.InnerException);
    }
  }
}
