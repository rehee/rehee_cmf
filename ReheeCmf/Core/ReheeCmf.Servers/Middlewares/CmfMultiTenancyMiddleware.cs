using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using ReheeCmf.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReheeCmf.Utility.CmfRegisters;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace ReheeCmf.Servers.Middlewares
{
  public class CmfMultiTenancyMiddleware : CmfMiddleware
  {
    public CmfMultiTenancyMiddleware(RequestDelegate next) : base(next) { }
    public override async Task InvokeAsync(HttpContext context, IServiceProvider sp)
    {
      TenantSetting setting = sp.GetService<TenantSetting>()!;
      IContextScope<Tenant> detail = sp.GetService<IContextScope<Tenant>>()!;
      ITenantService service = sp.GetService<ITenantService>()!;
      if (!setting.EnableTenant)
      {
        await next(context);
        return;
      }
      if (context.Request.Path.HasValue && context.Request.Path.ToString().StartsWith("/swagger"))
      {
        await next(context);
        return;

      }
      var tenant = service.GetTenant();
      if (tenant != null)
      {
        detail.SetValue(tenant);
        await next(context);
        return;
      }
      var controllerName = context.GetRouteValue("controller")?.ToString() ?? "";
      var actionName = context.GetRouteValue("action")?.ToString() ?? "";
      if (controllerName == null || actionName == null)
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        return;
      }
      if (!CmfRegister.TryGetController(controllerName, out var controller) || controller == null)
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        return;
      }
      var controllerMap = controller.GetMap();
      if (controllerMap.Attributes.Any(b => b is IgnoreTenantAttribute))
      {
        await next(context);
        return;
      }
      var action = controllerMap.Methods.FirstOrDefault(b => b.Name.Equals(actionName, StringComparison.OrdinalIgnoreCase));
      if (action != null && action.GetMap().Attributes.Any(b => b is IgnoreTenantAttribute))
      {
        await next(context);
        return;
      }
      context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
      return;
    }

  }
}
