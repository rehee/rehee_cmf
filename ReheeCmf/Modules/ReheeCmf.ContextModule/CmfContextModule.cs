using ReheeCmf.Contexts;
using ReheeCmf.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContextModule
{
  public class CmfContextModule<TContext, TUser>
    : ServiceModule
    where TContext : DbContext
    where TUser : IdentityUser, new()
  {
    public override string ModuleTitle => "CmfContextModule";

    public override string ModuleName => "CmfContextModule";

    public override Task<IEnumerable<string>> GetPermissions(IContext db, string token, CancellationToken ct)
    {
      return Task.FromResult(Enumerable.Empty<string>());
    }

    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      context.Services!.AddContextModule<TContext, TUser>(context.Configuration!);
      return Task.CompletedTask;
    }

  }
}
