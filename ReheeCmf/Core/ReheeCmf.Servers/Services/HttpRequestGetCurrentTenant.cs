using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Servers.Services
{
  public class HttpRequestGetCurrentTenant : IGetCurrentTenant
  {
    private readonly IHttpContextAccessor httpContextAccessor;

    public HttpRequestGetCurrentTenant(IHttpContextAccessor httpContextAccessor)
    {
      this.httpContextAccessor = httpContextAccessor;
    }
    public Guid? GetCurrentTenantId()
    {
      if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue(Common.TenantIDHeader, out var tenantString))
      {
        if (String.IsNullOrEmpty(tenantString) || !Guid.TryParse(tenantString, out var result))
        {
          return null;
        }
        return result;
      }
      return null;
    }
    public string? GetCurrentTenantName()
    {
      if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue(Common.TenantNameHeader, out var tenantString))
      {
        if (String.IsNullOrEmpty(tenantString))
        {
          return null;
        }
        return tenantString;
      }
      return null;
    }
  }
}
