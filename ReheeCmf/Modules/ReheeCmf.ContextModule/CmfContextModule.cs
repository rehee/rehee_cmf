using ReheeCmf.Contexts;
using ReheeCmf.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContextModule
{
  public class CmfContextModule : ServiceModule
  {
    public override string ModuleTitle => nameof(CmfContextModule);

    public override string ModuleName => nameof(CmfContextModule);

    public override Task<IEnumerable<string>> GetPermissions(IContext db, string token, CancellationToken ct)
    {
      throw new NotImplementedException();
    }
  }
}
