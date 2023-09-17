using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContextModule.Services
{
  public class CmfUserClaimsPrincipalFactory<TUser> : UserClaimsPrincipalFactory<TUser> where TUser : class
  {
    private readonly IContext? context;

    public CmfUserClaimsPrincipalFactory(IServiceProvider sp, UserManager<TUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
    {
      context = sp.GetService<IContext>();
    }

    //protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
    //{
    //  var userId = await UserManager.GetUserIdAsync(user).ConfigureAwait(false);
    //  var userName = await UserManager.GetUserNameAsync(user).ConfigureAwait(false);
    //  var id = new ClaimsIdentity("Identity.Application", // REVIEW: Used to match Application scheme
    //      Options.ClaimsIdentity.UserNameClaimType,
    //      Options.ClaimsIdentity.RoleClaimType);
    //  id.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
    //  id.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, userName!));
    //  if (UserManager.SupportsUserEmail)
    //  {
    //    var email = await UserManager.GetEmailAsync(user).ConfigureAwait(false);
    //    if (!string.IsNullOrEmpty(email))
    //    {
    //      id.AddClaim(new Claim(Options.ClaimsIdentity.EmailClaimType, email));
    //    }
    //  }
    //  var claims = await context.Query<TenantIdentityUserClaim>(true).Where(b => b.UserId == userId).ToArrayAsync();
    //  //if (UserManager.SupportsUserSecurityStamp)
    //  //{
    //  //  id.AddClaim(new Claim(Options.ClaimsIdentity.SecurityStampClaimType,
    //  //      await UserManager.GetSecurityStampAsync(user).ConfigureAwait(false)));
    //  //}
    //  //if (UserManager.SupportsUserClaim)
    //  //{
    //  //  id.AddClaims(await UserManager.GetClaimsAsync(user).ConfigureAwait(false));
    //  //}
    //  return id;
    //}
  }
}
