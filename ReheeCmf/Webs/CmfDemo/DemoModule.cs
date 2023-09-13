using CmfDemo.Data;
using Microsoft.AspNetCore.Identity;
using ReheeCmf;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.ContextModule;
using ReheeCmf.ContextModule.Contexts;
using ReheeCmf.ContextModule.Entities;
using ReheeCmf.Contexts;
using ReheeCmf.EntityModule;
using ReheeCmf.Modules;

namespace CmfDemo
{
  public class DemoModule : CmfApiModule
  {
    public override IEnumerable<ModuleDependOn> Depends()
    {
      return base.Depends().Concat(new ModuleDependOn[]
      {
        ModuleDependOn.New<CmfContextModule<ApplicationDbContext,ReheeCmfBaseUser>>(),
        ModuleDependOn.New<CmfEntityModule>()
      });
    }
    public override string ModuleTitle => "";

    public override string ModuleName => "";


    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.ConfigureServicesAsync(context);

    }

    public override Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {
      return Task.FromResult(Array.Empty<string>().AsEnumerable());
    }
  }
}
