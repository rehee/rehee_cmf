using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Authenticates;
using ReheeCmf.Helpers;
using ReheeCmf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.UserManagementModule.Services
{
  public class CmfUserRoleService<TUser, TRole> : IUserRoleService
    where TUser : IdentityUser, new()
    where TRole : IdentityRole, new()
  {
    private readonly UserManager<TUser> userManager;
    private readonly RoleManager<TRole> roleManager;
    private readonly IAsyncQuery asyncQuery;
    private readonly UserManagementOption option;

    public CmfUserRoleService(IServiceProvider sp)
    {
      this.userManager = sp.GetService<UserManager<TUser>>()!;
      this.roleManager = sp.GetService<RoleManager<TRole>>()!;
      asyncQuery = sp.GetService<IAsyncQuery>()!;
      option = sp.GetService<UserManagementOption>()!;
    }
    public async Task UserRoleAsync(object user, Dictionary<string, string?> properities, CancellationToken ct = default)
    {
      if (String.IsNullOrEmpty(option.RoleProperty))
      {
        return;
      }
      if (!properities.TryGetValueStringKey(option.RoleProperty!, out var roleValue) || String.IsNullOrEmpty(roleValue))
      {
        return;
      }
      if (user is TUser != true)
      {
        return;
      }
      string[]? rolesFromProperty = null;
      if (option.RoleIsMulti)
      {

        rolesFromProperty = roleValue.GetValue<string[]>();
      }
      else
      {
        rolesFromProperty = new string[] { roleValue };
      }
      var roles = asyncQuery.AsNoTracking(roleManager.Roles).Select(b => b.Name).ToArray();
      var roleChecks = roles.Where(b => rolesFromProperty!.Any(r => String.Equals(r, b, StringComparison.OrdinalIgnoreCase))).ToArray();
      if (roleChecks?.Length <= 0)
      {
        return;
      }
      var typedUser = (user as TUser)!;
      var allRoles = await userManager.GetRolesAsync(typedUser);
      var rolesNeedDelete = allRoles.Where(b => !roleChecks!.Any(r => String.Equals(b, r, StringComparison.OrdinalIgnoreCase))).ToArray();
      await userManager.RemoveFromRolesAsync(typedUser, rolesNeedDelete);
      var rolesNeedAdd = roleChecks!.Where(b => !allRoles!.Any(r => String.Equals(b, r, StringComparison.OrdinalIgnoreCase))).ToArray();
      await userManager.AddToRolesAsync(typedUser, rolesNeedAdd);
    }
  }
}
