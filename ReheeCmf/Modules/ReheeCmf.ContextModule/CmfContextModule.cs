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


    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      context.Services!.AddContextModule<TContext, TUser>(context.Configuration!);
      return Task.CompletedTask;
    }

    public override Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {
      return Task.FromResult(Enumerable.Empty<string>());
    }
  }
}
