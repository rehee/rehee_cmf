using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ReheeCmf.Entities;
using ReheeCmf.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ReheeCmf.Authenticates;
using ReheeCmf.Responses;
using ReheeCmf.Commons.Consts;

namespace ReheeCmf.Servers.Filters
{
  public class CmfAuthorizationFilter : IAsyncActionFilter
  {
    private readonly IAuthorize auth;
    private readonly IContextScope<Tenant> tenant;
    private readonly IContextScope<TokenDTO> scopeUser;
    private readonly IJWTService jwt;
    private readonly TenantSetting tenantSetting;
    private readonly IDTOSignInManager<ClaimsPrincipalDTO> tus;

    public CmfAuthorizationFilter(IAuthorize auth, IJWTService jwt, IContextScope<Tenant> tenant, IContextScope<TokenDTO> scopeUser, TenantSetting tenantSetting, IDTOSignInManager<ClaimsPrincipalDTO> tus)
    {
      this.auth = auth;
      this.jwt = jwt;
      this.tenant = tenant;
      this.scopeUser = scopeUser;
      this.tenantSetting = tenantSetting;
      this.tus = tus;
    }


    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      if (!await auth.EnableAuth())
      {
        goto gotoNext;
      }
      if (context.ActionDescriptor.GetType() != typeof(Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor))
      {
        goto gotoNext;
      }
      var actionInfo = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
      var controller = actionInfo.ControllerTypeInfo;
      MethodInfo action;
      try
      {
        action = controller.GetMethod(actionInfo.ActionName, actionInfo.Parameters.Select(b => b.ParameterType).ToArray());
      }
      catch (Exception ex)
      {
        context.Result = new StatusCodeResult(401);
        return;
      }
      if (action == null)
      {
        goto gotoNext;
      }
      var allow = action.GetCustomAttribute<AllowAnonymousAttribute>();
      if (allow != null)
      {
        goto gotoNext;
      }
      var cmfAttr = action.GetCustomAttribute<CmfAuthorizeAttribute>();
      if (cmfAttr == null)
      {
        goto gotoNext;
      }
      IEnumerable<string> permissionsFromToken = Enumerable.Empty<string>();
      string token;
      ContentResponse<TokenDTO> validateTokenResult = new ContentResponse<TokenDTO>();
      if (context.HttpContext.Request.Headers.TryGetValue(ConstOptions.AuthorizeHeader, out var t))
      {
        token = t.FirstOrDefault();
        validateTokenResult = await auth.ValidateAndConvert(token);
        if (!validateTokenResult.Success)
        {
          context.Result = new StatusCodeResult(401);
          return;
        }
        var user = validateTokenResult.Content;
        scopeUser.SetValue(user);
        var systemApi = user.IsSystemToken;
        if (systemApi == true)
        {
          goto gotoNext;
        }
        var tId = tenant?.Value.TenantID?.ToString() ?? Common.EmptyGuid;
        var tokenTid = user?.TenantID?.ToString() ?? Common.EmptyGuid;
        if (!tId.Equals(tokenTid))
        {
          context.Result = new StatusCodeResult(401);
          return;
        }

        var fullAccessRole = await auth.FullAccessRole();
        if (!String.IsNullOrEmpty(fullAccessRole))
        {
          user.Roles.Contains(fullAccessRole);
          goto gotoNext;
        }
        permissionsFromToken = user.Permissions.Where(b => !String.IsNullOrEmpty(b)).ToArray();
      }
      else if (context.HttpContext.User?.Identity?.IsAuthenticated == true)
      {
        if (tenantSetting.EnableTenant && tenant.Value?.TenantID != null)
        {
          var withTenant = context.HttpContext.User.TryGetClaim(Common.TenantIDHeader, out var tenantIdString);
          if (withTenant == true && Guid.TryParse(tenantIdString, out var userGUid))
          {
            if (!userGUid.Equals(tenant.Value.TenantID))
            {
              await tus.LogoutAsync(CancellationToken.None);
              goto startvalidation;
            }
          }
          else
          {
            var login = await tus.LoginAsync(new ClaimsPrincipalDTO { User = context.HttpContext.User, KeepLogin = true }, CancellationToken.None);
            if (!login.Success)
            {
              goto startvalidation;
            }
          }
        }

        var userName = context.HttpContext.User.Identity.Name;
        validateTokenResult = await jwt.GetTokenDTO(userName);
        if (validateTokenResult.Success)
        {
          scopeUser.SetValue(validateTokenResult.Content);
        }
      }
    startvalidation:


      if (cmfAttr.AuthOnly)
      {
        if (validateTokenResult?.Success != true)
        {
          context.Result = new StatusCodeResult(401);
          return;
        }
        goto gotoNext;
      }
      string entityName = string.Empty;
      if (!String.IsNullOrEmpty(cmfAttr.EntityName))
      {
        context.ActionArguments.TryGetValue(cmfAttr.EntityName, out var actionArguments);
        context.RouteData.Values.TryGetValue(cmfAttr.EntityName, out var routeData);
        entityName = routeData?.StringValue() ?? actionArguments?.StringValue() ?? null;
      }

      string actionName = string.Empty;
      if (!String.IsNullOrEmpty(cmfAttr.EntityAction) && context.ActionArguments.TryGetValue(cmfAttr.EntityAction, out var aName))
      {
        actionName = aName.StringValue();
      }
      if (context.Controller != null && context.Controller is IWithEntityName cn)
      {
        entityName = cn.EntityName;
      }
      if (cmfAttr.PermissionClass)
      {

        var key = ReflectPool.NamePermissionMap.Keys.FirstOrDefault(b => b.Equals(entityName, StringComparison.OrdinalIgnoreCase));
        if (String.IsNullOrEmpty(key))
        {
          context.Result = new StatusCodeResult(403);
          return;
        }
        if (ReflectPool.NamePermissionMap.TryGetValue(key, out var p))
        {
          if (p.Permissions?.Any() != true)
          {
            if (permissionsFromToken.Contains(p.Permission))
            {
              goto gotoNext;
            }
          }
          else
          {
            EnumHttpMethod? classEnumHttpMethod = null;
            switch (context.HttpContext.Request.Method.ToLower())
            {
              case "get":
                classEnumHttpMethod = EnumHttpMethod.Get;
                break;
              case "post":
                classEnumHttpMethod = EnumHttpMethod.Post;
                break;
              case "put":
                classEnumHttpMethod = EnumHttpMethod.Put;
                break;
              case "delete":
                classEnumHttpMethod = EnumHttpMethod.Delete;
                break;
            }
            var permissionFromClassMethod = p.Permissions.FirstOrDefault(b => b.Method == classEnumHttpMethod);
            if (permissionFromClassMethod != null && permissionsFromToken.Contains(permissionFromClassMethod.Permission))
            {
              goto gotoNext;
            }
          }
        }

        context.Result = new StatusCodeResult(403);
        return;
      }
      if (cmfAttr.PermissionAction)
      {
        var actionP = ReflectPool.GetEntityNameActionPermission(entityName, actionName);
        if (!String.IsNullOrEmpty(actionP?.Permission) && !permissionsFromToken.Contains(actionP?.Permission))
        {
          //StatusException.Throw(HttpStatusCode.Unauthorized, "not in permission action");
          context.Result = new StatusCodeResult(403);
          return;
        }
        goto gotoNext;
      }
      if (!String.IsNullOrEmpty(cmfAttr.EntityNameForce))
      {
        entityName = cmfAttr.EntityNameForce;
        if (permissionsFromToken.Contains(entityName))
        {
          goto gotoNext;
        }
        //StatusException.Throw(HttpStatusCode.Unauthorized, "not in permission entity");
        context.Result = new StatusCodeResult(403);
        return;

      }
      var isEntityKey = context.ActionArguments.TryGetValue(cmfAttr.EntityKey, out var entityKey);
      var permissionString = cmfAttr.RoleString;
      if (cmfAttr.EntityRoleBase)
      {
        permissionString = $"{entityName}:";
        switch (context.HttpContext.Request.Method.ToLower())
        {
          case "get":
            if (permissionString.IndexOf(nameof(RoleBasedPermission)) == 0)
            {
              return;
            }
            permissionString = $"{permissionString}read";
            break;
          case "post":
            permissionString = $"{permissionString}create";
            break;
          case "put":
            permissionString = $"{permissionString}update";
            break;
          case "delete":
            permissionString = $"{permissionString}delete";
            break;
        }
      }
      if (permissionsFromToken.Any(b => b.Equals(permissionString, StringComparison.OrdinalIgnoreCase)))
      {
        goto gotoNext;
      }
      context.Result = new StatusCodeResult(403);
      return;
    gotoNext:
      await next();
    }
  }

}
