using Microsoft.AspNetCore.Identity;
using ReheeCmf.Commons.Consts;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Contexts;
using ReheeCmf.Entities;
using ReheeCmf.Modules;
using ReheeCmf.Modules.Helpers;
using ReheeCmf.Modules.Permissions;

namespace ReheeCmf.AuthenticationModule
{
  public class CmfAuthenticationModule<TUser> : ServiceModule where TUser : IdentityUser, ICmfUser, new()
  {
    public override string ModuleTitle => ConstModule.CmfAuthenticationModule;

    public override string ModuleName => ConstModule.CmfAuthenticationModule;
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.ConfigureServicesAsync(context);
      context.Services!.AddCmfAuthentication<TUser>(context.Configuration!);
    }

    public override Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {
      return Task.FromResult(ModulePermissionComponentHelper.GetPermission<ConstCmfAuthenticationModule>()!);
    }
  }
}
