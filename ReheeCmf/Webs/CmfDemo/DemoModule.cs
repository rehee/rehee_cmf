using ReheeCmf;
using ReheeCmf.Contexts;
using ReheeCmf.Libs.Modules;

namespace CmfDemo
{
  public class DemoModule : CmfApiModule
  {
    public override string ModuleTitle => "";

    public override string ModuleName => "";

    public override Task<IEnumerable<string>> GetPermissions(IContext db, string token, CancellationToken ct)
    {
      return Task.FromResult(Array.Empty<string>().AsEnumerable());
    }
  }
}
