using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContextModule.Managers
{
  public class CmfSignInManager<TUser> : SignInManager<TUser> where TUser : class
  {
    public CmfSignInManager(UserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<TUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {

    }
    public async override Task SignInAsync(TUser user, bool isPersistent, string authenticationMethod = null)
    {
      await base.SignInAsync(user, isPersistent, authenticationMethod);
    }

    public async override Task SignInAsync(TUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
    {
      await base.SignInAsync(user, authenticationProperties, authenticationMethod);
    }
  }
}
