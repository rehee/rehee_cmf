using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.ContextComponent;
using ReheeCmf.ContextModule.Components;
using ReheeCmf.Contexts;
using ReheeCmf.Handlers.ContextHandlers;
using System.Reflection.Emit;

namespace ReheeCmf.ContextModule.Contexts
{
  [ContextFactoryComponent<CmfDbContextBuilder>]
  public class CmfIdentityContext<TUser> : IdentityDbContext<
    TUser,
    TenantIdentityRole,
    string,
    TenantIdentityUserClaim,
    TenantIdentityUserRole,
    TenantIdentityUserLogin,
    TenantIdentityRoleClaim,
    TenantIdentityUserToken>, IIdentityContext, IWithContext, ITenantContext, ITokenDTOContext
    where TUser : IdentityUser, ICmfUser, new()
  {
    protected readonly IServiceProvider sp;
    public IContext? Context { get; set; }
    protected readonly IContextScope<Tenant> scopeTenant;
    protected readonly IContextScope<TokenDTO> scopeUser;
    public CmfIdentityContext(IServiceProvider sp)
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
      builder.Entity<TenantIdentityUserClaim>().HasKey(b => b.Id);

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
      scopeTenant.ValueChange -= ScopeTenant_ValueChange;
      scopeUser.ValueChange -= ScopeUser_ValueChange;
      ThisTenant = null;
      CrossTenant = null;
      IsDispose = true;
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
    public override DbSet<TenantIdentityUserClaim> UserClaims { get; set; }
    public DbSet<RoleBasedPermission> RoleBasedPermissions { get; set; }


  }
}
