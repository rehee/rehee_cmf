using ReheeCmf.ContextComponent;
using ReheeCmf.ContextModule.Components;
using ReheeCmf.Handlers.ContextHandlers;

namespace ReheeCmf.ContextModule.Contexts
{

  [ContextFactoryComponentAttribute<CmfDbContextBuilder>]
  public class CmfDbContext : DbContext, IWithContext, ITenantContext, ITokenDTOContext
  {
    protected readonly IServiceProvider sp;
    private readonly IContextScope<Tenant> scopeTenant;
    protected readonly IContextScope<TokenDTO> scopeUser;
    public IContext? Context { get; set; }
    public CmfDbContext(IServiceProvider sp)
    {
      this.sp = sp;
      scopeTenant = sp.GetService<IContextScope<Tenant>>()!;
      SetTenant(scopeTenant!.Value);
      scopeTenant.ValueChange += ScopeTenant_ValueChange;
      scopeUser = sp.GetService<IContextScope<TokenDTO>>()!;
      SetUser(scopeUser!.Value);
      scopeUser.ValueChange += ScopeUser_ValueChange;
    }
    private void ScopeUser_ValueChange(object? sender, ContextScopeEventArgs<TokenDTO> e)
    {
      SetUser(e.Value);
    }
    private void ScopeTenant_ValueChange(object? sender, ContextScopeEventArgs<Tenant> e)
    {
      SetTenant(e.Value);
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
      scopeTenant.ValueChange -= ScopeTenant_ValueChange;
      ThisTenant = null;
      CrossTenant = null;
      if (Context != null)
      {

        Context.Dispose();
      }
    }

    public Tenant? ThisTenant { get; protected set; }

    public bool IgnoreTenant { get; protected set; }

    public Guid? TenantID
    {
      get => ThisTenant?.TenantID;
      set { }
    }

    public Guid? CrossTenantID => CrossTenant?.TenantID;

    public Tenant? CrossTenant { get; protected set; }
    public void SetTenant(Tenant? tenant)
    {
      ThisTenant = tenant;
    }

    public void SetReadOnly(bool readOnly)
    {

    }

    public void SetIgnoreTenant(bool ignore)
    {
      IgnoreTenant = ignore;
    }

    public void SetCrossTenant(Tenant? tenant)
    {
      CrossTenant = tenant;
    }
    public TokenDTO? User { get; protected set; }
    public void SetUser(TokenDTO? user)
    {
      User = user;
    }
    public DbSet<TenantEntity> Tenants { get; set; }
    public DbSet<RoleBasedPermission> RoleBasedPermissions { get; set; }



  }
}
