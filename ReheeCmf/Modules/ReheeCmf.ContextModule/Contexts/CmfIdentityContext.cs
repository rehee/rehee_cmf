using ReheeCmf.ContextComponent;
using ReheeCmf.ContextModule.Components;
using ReheeCmf.Helper;

namespace ReheeCmf.ContextModule.Contexts
{
  [CmfComponent<CmfDbContextBuilder>]
  public class CmfIdentityContext<TUser> : IdentityDbContext<
    TUser,
    TenantIdentityRole,
    string,
    TenantIdentityUserClaim,
    TenantIdentityUserRole,
    TenantIdentityUserLogin,
    TenantIdentityRoleClaim,
    TenantIdentityUserToken>, IIdentityContext, IWithContext
    where TUser : IdentityUser, ICmfUser, new()
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
      var handler = this.GetType().GetComponentsByHandler<IDbContextBuilder>().OrderBy(b => b.Index);
      foreach (var h in handler)
      {
        h.SingletonHandler<IDbContextBuilder>()?.OnConfiguring(optionsBuilder, sp, this);
      }
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
    }

    public override void Dispose()
    {
      SelfDispose();
      base.Dispose();

    }
    public override async ValueTask DisposeAsync()
    {
      SelfDispose();
      await base.DisposeAsync();

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

    public DbSet<TenantEntity> Tenants { get; set; }
    public DbSet<RoleBasedPermission> RoleBasedPermissions { get; set; }

  }
}
