using ReheeCmf.Commons.DTOs;
using ReheeCmf.Modules;

namespace ReheeCmf.ContentManagementModule
{
  public class CmfContentManagememntModule : ServiceModule
  {
    public override string ModuleTitle => nameof(CmfContentManagememntModule);

    public override string ModuleName => nameof(CmfContentManagememntModule);

    public override async Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {
      await Task.CompletedTask;
      return [];
    }
  }
}
