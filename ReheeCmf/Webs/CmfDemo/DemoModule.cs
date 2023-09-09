using CmfDemo.Data;
using Microsoft.AspNetCore.Identity;
using ReheeCmf;
using ReheeCmf.ContextModule;
using ReheeCmf.ContextModule.Contexts;
using ReheeCmf.Contexts;
using ReheeCmf.Modules;

namespace CmfDemo
{
  public class DemoModule : CmfApiModule
  {
    public override IEnumerable<ModuleDependOn> Depends()
    {
      return base.Depends().Concat(new ModuleDependOn[]
      {
        ModuleDependOn.New<CmfContextModule<ApplicationDbContext,IdentityUser>>()
      });
    }
    public override string ModuleTitle => "";

    public override string ModuleName => "";

    public override Task<IEnumerable<string>> GetPermissions(IContext db, string token, CancellationToken ct)
    {
      return Task.FromResult(Array.Empty<string>().AsEnumerable());
    }
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.ConfigureServicesAsync(context);
      context.Services!.AddDbContext<ApplicationDbContext>();
      context.Services!.AddScoped<IContext, CmfRepositoryContext>(sp =>
        new CmfRepositoryContext(sp, sp.GetService<ApplicationDbContext>()!));
    }
  }
}
