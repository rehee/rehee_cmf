using ReheeCmf.Commons.Consts;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Contexts;
using ReheeCmf.Modules;

namespace ReheeCmf.AuthenticationModule
{
  public class CmfAuthenticationModule : ServiceModule
  {
    public override string ModuleTitle => nameof(CmfAuthenticationModule);

    public override string ModuleName => nameof(CmfAuthenticationModule);

    public override Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {
      var result = new string[]
      {
        ConstAuthentication.PermissionReadRoleBasedAccess,
        ConstAuthentication.PermissionUpdateRoleBasedAccess
      };
      return Task.FromResult(result as IEnumerable<string>);
    }
  }
}
