using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Servers.Middlewares
{
  public abstract class CmfMiddleware
  {
    protected readonly RequestDelegate next;

    public CmfMiddleware(RequestDelegate next)
    {
      this.next = next;
    }
    public abstract Task InvokeAsync(HttpContext context, IServiceProvider sp);
  }
}
