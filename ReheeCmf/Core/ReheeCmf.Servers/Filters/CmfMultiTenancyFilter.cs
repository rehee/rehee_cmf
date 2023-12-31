﻿using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ReheeCmf.Attributes;

namespace ReheeCmf.Servers.Filters
{
  public class CmfMultiTenancyFilter : IAsyncAuthorizationFilter
  {
    private readonly TenantSetting setting;
    private readonly IContextScope<Tenant> detail;
    private readonly ITenantService service;

    public CmfMultiTenancyFilter(TenantSetting setting, IContextScope<Tenant> detail, ITenantService service)
    {
      this.setting = setting;
      this.detail = detail;
      this.service = service;
    }
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
      await Task.CompletedTask;
      if (!setting.EnableTenant)
      {
        return;
      }

      var actions = context.ActionDescriptor;
      if (actions is ControllerActionDescriptor mac)
      {
        var controller = mac.ControllerTypeInfo;
        var map = controller.GetMap();
        if (map.Attributes.Any(b => b is IgnoreTenantAttribute))
        {
          return;
        }
        var actionSelected = map.Methods.FirstOrDefault(b => b.Name == mac.ActionName);
        if (actionSelected != null && actionSelected.GetMap().Attributes.Any(b => b is IgnoreTenantAttribute))
        {
          return;
        }
      }
      var tenant = service.GetTenant();
      if (tenant == null)
      {
        context.Result = new StatusCodeResult(401);
        return;
      }
      if (detail != null)
      {
        detail.SetValue(tenant);
      }
    }
  }


  //public class CmfMultiTenancyFilter : IAsyncActionFilter
  //{
  //  private readonly TenantSetting setting;
  //  private readonly IContextScope<Tenant> detail;
  //  private readonly ITenantService service;

  //  public CmfMultiTenancyFilter(TenantSetting setting, IContextScope<Tenant> detail, ITenantService service)
  //  {
  //    this.setting = setting;
  //    this.detail = detail;
  //    this.service = service;
  //  }
  //  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  //  {
  //    var c = context.Controller;
  //    if (context.Controller != null)
  //    {
  //      var controller = context.Controller.GetMap();
  //      if (controller.Attributes.Any(b => b is IgnoreTenantAttribute))
  //      {
  //        await next();
  //        return;
  //      }
  //      if (context.ActionDescriptor is ControllerActionDescriptor mac)
  //      {
  //        var action = controller.Methods.FirstOrDefault(b => b.Name.Equals(mac.ActionName));
  //        if (action != null && action.GetMap().Attributes.Any(b => b is IgnoreTenantAttribute))
  //        {
  //          await next();
  //          return;
  //        }
  //      }
  //    }


  //    if (!setting.EnableTenant)
  //    {
  //      await next();
  //      return;
  //    }
  //    var tenant = service.GetTenant();
  //    if (tenant == null)
  //    {
  //      context.Result = new StatusCodeResult(401);
  //      return;
  //    }
  //    if (detail != null)
  //    {
  //      detail.SetValue(tenant);
  //    }
  //    await next();
  //  }
  //}
}
