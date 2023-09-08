using ReheeCmf.ContextComponent;
using ReheeCmf.ContextModule.Components;
using ReheeCmf.Helper;

namespace ReheeCmf.ContextModule.Contexts
{
  [ComponentHandler<CmfDbContextBuilder>(Index = 2)]
  public class CmfIdentityContext<TUser> : IdentityDbContext<
    TUser,
    TenantIdentityRole,
    string,
    TenantIdentityUserClaim,
    TenantIdentityUserRole,
    TenantIdentityUserLogin,
    TenantIdentityRoleClaim,
    TenantIdentityUserToken>, IIdentityContext, IWithContext
    where TUser : IdentityUser, new()
  {
    protected readonly IServiceProvider sp;
    public IContext? Context { get; set; }
    public CmfIdentityContext(IServiceProvider sp)
    {
      this.sp = sp;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
      var handler = this.GetType().GetComponentHandler<IDbContextBuilder>().ToArray().Reverse();
      foreach (var h in handler)
      {
        h.SingletonComponent<IDbContextBuilder>()?.OnConfiguring(optionsBuilder, Context);
      }
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
    }

    public override void Dispose()
    {
      base.Dispose();
      SelfDispose();
    }
    public override async ValueTask DisposeAsync()
    {
      await base.DisposeAsync();
      SelfDispose();
    }
    bool IsDispose { get; set; }
    protected void SelfDispose()
    {
      if (IsDispose)
      {
        return;
      }
      IsDispose = true;
      if (Context != null)
      {
        Context.Dispose();
      }
    }
  }
}
