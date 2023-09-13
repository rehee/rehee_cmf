using ReheeCmf.Commons.DTOs;
using ReheeCmf.Contexts;
using ReheeCmf.Modules;

namespace ReheeCmf.EntityModule
{
  public class CmfEntityModule : ServiceModule
  {
    public override string ModuleTitle => nameof(CmfEntityModule);

    public override string ModuleName => nameof(CmfEntityModule);

    public override Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {
      IEnumerable<string> result = new string[]
      {

      };
      return Task.FromResult(result);
    }
  }
}
