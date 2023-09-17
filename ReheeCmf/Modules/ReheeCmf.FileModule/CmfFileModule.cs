using ReheeCmf.Commons.DTOs;
using ReheeCmf.Contexts;
using ReheeCmf.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.FileModule
{
  public class CmfFileModule : ServiceModule
  {
    public override string ModuleTitle => ConstModule.CmfFileModule;

    public override string ModuleName => ConstModule.CmfFileModule;

    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.ConfigureServicesAsync(context);
      context.Services!.AddCmfFileService(context.Configuration!);
    }
    public override Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {
      IEnumerable<string> result = new string[]
      {

      };

      return Task.FromResult(result);
    }
  }
}
