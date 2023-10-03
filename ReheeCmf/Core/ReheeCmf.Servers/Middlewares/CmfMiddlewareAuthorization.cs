using Microsoft.AspNetCore.Http;
using ReheeCmf.Commons.Consts;
using ReheeCmf.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Servers.Middlewares
{
  public class CmfMiddlewareAuthorization : CmfMiddleware
  {
    public CmfMiddlewareAuthorization(RequestDelegate next) : base(next)
    {
    }

    public override async Task InvokeAsync(HttpContext context, IServiceProvider sp)
    {
      IAuthorize auth = sp.GetService<IAuthorize>()!;
      IContextScope<Tenant> tenant = sp.GetService<IContextScope<Tenant>>()!;
      IContextScope<TokenDTO> scopeUser = sp.GetService<IContextScope<TokenDTO>>()!;
      IJWTService jwt = sp.GetService<IJWTService>()!;
      TenantSetting tenantSetting = sp.GetService<TenantSetting>()!;
      IDTOSignInManager<ClaimsPrincipalDTO> tus = sp.GetService<IDTOSignInManager<ClaimsPrincipalDTO>>()!;
      string? token;
      ContentResponse<TokenDTO>? validateTokenResult = null;
      if (context.Request.Headers.TryGetValue(ConstOptions.AuthorizeHeader, out var t))
      {
        token = t.FirstOrDefault();
        if (String.IsNullOrEmpty(token))
        {
          await next(context);
          return;
        }
        validateTokenResult = await auth.ValidateAndConvert(token);
        if (!validateTokenResult.Success)
        {
          await next(context);
          return;
        }
        scopeUser.SetValue(validateTokenResult.Content);
        await next(context);
        return;
      }
      else if (context.User?.Identity?.IsAuthenticated == true)
      {
        if (tenantSetting.EnableTenant && tenant.Value?.TenantID != null)
        {
          var withTenant = context.User.TryGetClaim(Common.TenantIDHeader, out var tenantIdString);
          if (withTenant == true && Guid.TryParse(tenantIdString, out var userGUid))
          {
            if (!userGUid.Equals(tenant.Value.TenantID))
            {
              await tus.LogoutAsync(CancellationToken.None);
              await next(context);
              return;
            }
          }
          else
          {
            var login = await tus.LoginAsync(new ClaimsPrincipalDTO { User = context.User, KeepLogin = true }, CancellationToken.None);
            if (!login.Success)
            {
              await next(context);
              return;
            }
          }
        }
        var userName = context.User.Identity.Name!;
        validateTokenResult = await jwt.GetTokenDTO(userName);
        if (validateTokenResult.Success)
        {
          scopeUser.SetValue(validateTokenResult.Content);
          await next(context);
          return;
        }
      }

      await next(context);
      return;
    }
  }
}
