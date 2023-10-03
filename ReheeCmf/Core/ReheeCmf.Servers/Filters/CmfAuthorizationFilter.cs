using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ReheeCmf.Entities;
using ReheeCmf.Enums;
using System.Reflection;
using ReheeCmf.Responses;
using ReheeCmf.Commons.Consts;

namespace ReheeCmf.Servers.Filters
{
  public class CmfAuthorizationActionFilter : IAsyncActionFilter
  {
    private readonly IAuthorize auth;
    private readonly IContextScope<Tenant> tenant;
    private readonly IContextScope<TokenDTO> scopeUser;
    private readonly IJWTService jwt;
    private readonly TenantSetting tenantSetting;
    private readonly IDTOSignInManager<ClaimsPrincipalDTO> tus;

    public CmfAuthorizationActionFilter(IAuthorize auth, IJWTService jwt, IContextScope<Tenant> tenant, IContextScope<TokenDTO> scopeUser, TenantSetting tenantSetting, IDTOSignInManager<ClaimsPrincipalDTO> tus)
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
      if (scopeUser.Value != null)
      {
        permissionsFromToken = scopeUser.Value.Permissions ?? Enumerable.Empty<string>();
      }
      if (cmfAttr.AuthOnly)
      {
        if (scopeUser.Value == null)
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
        permissionString = $"{entityName}{ConstCrud.Split}";
        switch (context.HttpContext.Request.Method.ToLower())
        {
          case "get":
            if (permissionString.IndexOf(nameof(RoleBasedPermission)) == 0)
            {
              return;
            }
            permissionString = $"{permissionString}{ConstCrud.Read}";
            break;
          case "post":
            permissionString = $"{permissionString}{ConstCrud.Create}";
            break;
          case "put":
            permissionString = $"{permissionString}{ConstCrud.Update}";
            break;
          case "delete":
            permissionString = $"{permissionString}{ConstCrud.Delete}";
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
