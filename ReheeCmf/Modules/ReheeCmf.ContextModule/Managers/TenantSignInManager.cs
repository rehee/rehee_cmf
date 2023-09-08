using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReheeCmf.Commons;
using ReheeCmf.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContextModule.Managers
{
  public class TenantSignInManager<TUser> : SignInManager<TUser> where TUser : IdentityUser, new()
  {
    private readonly IContext context;

    public TenantSignInManager(IContext context, UserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<TUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
      this.context = context;
    }

    public override async Task<TUser> ValidateSecurityStampAsync(ClaimsPrincipal principal)
    {
      try
      {
        context.SetIgnoreTenant(true);
        return await base.ValidateTwoFactorSecurityStampAsync(principal);
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
