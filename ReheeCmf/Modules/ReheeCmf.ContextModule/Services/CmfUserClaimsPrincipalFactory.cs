using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ReheeCmf.Commons.Consts;
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

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
    {
      var result = await base.GenerateClaimsAsync(user);
      if (!result.Claims.Any(c => c.Type == ConstOptions.RoleType))
      {
        var roles = await UserManager.GetRolesAsync(user);
        if (roles?.Any() == true)
        {
          foreach (var role in roles)
          {
            result.AddClaim(new Claim(ConstOptions.RoleType, role));
          }
        }

      }
      return result;
    }
  }
}
