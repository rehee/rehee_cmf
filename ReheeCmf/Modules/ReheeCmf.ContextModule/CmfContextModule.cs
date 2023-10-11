using ReheeCmf.Modules;

namespace ReheeCmf.ContextModule
{
  public class CmfContextModule<TContext, TUser>
    : ServiceModule
    where TContext : DbContext
    where TUser : IdentityUser, ICmfUser, new()
  {
    public override string ModuleTitle => ConstModule.CmfContextModule;
    public override string ModuleName => ConstModule.CmfContextModule;


    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.ConfigureServicesAsync(context);
      context.Services!.AddContextModule<TContext, TUser>(context.Configuration!);
    }

    public override Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {
      return Task.FromResult(Enumerable.Empty<string>());
    }
  }
}
