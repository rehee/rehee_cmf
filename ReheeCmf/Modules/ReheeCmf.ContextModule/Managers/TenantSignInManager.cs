using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ReheeCmf.ContextModule.Managers
{
  public class TenantSignInManager<TUser> : SignInManager<TUser> where TUser : IdentityUser, new()
  {
    private readonly IContext context;
    private readonly ITenantService tenantService;

    public TenantSignInManager(IContext context, ITenantService tenantService, UserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<TUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
      this.context = context;
      this.tenantService = tenantService;
    }

    public override async Task<TUser> ValidateSecurityStampAsync(ClaimsPrincipal principal)
    {
      try
      {
        if (Guid.TryParse(principal.Claims.FirstOrDefault(b => b.Type == Common.TenantIDHeader)?.Value, out var tenantId))
        {
          var tenant = tenantService.GetTenantById(tenantId);
          if (tenant != null)
          {
            context.SetTenant(tenant);
          }
          context.SetIgnoreTenant(true);
          if (principal == null || principal.Identity?.Name == null)
          {
            return null;
          }
          var name = principal.Identity.Name.ToUpper();
          var users = await UserManager.Users.Where(b => b.NormalizedUserName == name).AsNoTracking().ToArrayAsync();
          TUser? user = null;
          if (typeof(TUser).IsImplement<IWithTenant>())
          {
            user = users.Select(b => b as IWithTenant).FirstOrDefault(b => b.TenantID == tenantId) as TUser;
          }
          if (user == null)
          {
            return null;
          }
          if (await ValidateSecurityStampAsync(user, principal.FindFirstValue(Options.ClaimsIdentity.SecurityStampClaimType)))
          {
            return user;
          }
          return null;
        }
        else
        {
          var result = await base.ValidateSecurityStampAsync(principal);
          return result;
        }
      }
      catch
      {
        StatusException.Throw(System.Net.HttpStatusCode.InternalServerError, "Error on Validate");
        return default(TUser);
      }
      finally
      {
        context.SetIgnoreTenant(false);
      }
    }
    public override async Task<bool> ValidateSecurityStampAsync(TUser user, string securityStamp)
    {
      try
      {
        if (user is IWithTenant tenantUser)
        {
          var tenant = tenantService.GetTenantById(tenantUser.TenantID);
          if (tenant != null)
          {
            context.SetTenant(tenant);
          }
        }

        context.SetIgnoreTenant(true);
        return await base.ValidateSecurityStampAsync(user, securityStamp);
      }
      catch
      {
        StatusException.Throw(System.Net.HttpStatusCode.InternalServerError, "Error on Validate");
        return false;
      }
      finally
      {
        context.SetIgnoreTenant(false);
      }
    }
    public override async Task<TUser> ValidateTwoFactorSecurityStampAsync(ClaimsPrincipal principal)
    {
      try
      {
        if (Guid.TryParse(principal.Claims.FirstOrDefault(b => b.Type == Common.TenantIDHeader)?.Value, out var tenantId))
        {
          var tenant = tenantService.GetTenantById(tenantId);
          if (tenant != null)
          {
            context.SetTenant(tenant);
          }
        }
        context.SetIgnoreTenant(true);
        return await base.ValidateTwoFactorSecurityStampAsync(principal);
      }
      catch
      {
        StatusException.Throw(System.Net.HttpStatusCode.InternalServerError, "Error on Validate");
        return null;
      }
      finally
      {
        context.SetIgnoreTenant(false);
      }

    }

  }

}
