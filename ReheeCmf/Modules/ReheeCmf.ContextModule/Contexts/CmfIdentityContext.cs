using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.ContextComponent;
using ReheeCmf.ContextModule.Components;
using ReheeCmf.Contexts;
using ReheeCmf.Handlers.ContextHandlers;
using ReheeCmf.MultiTenants;
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
    protected readonly CrudOption? crudOption;
    protected readonly TenantConnection? tenantConnection;
    public bool? ReadOnly { get; set; }
    public CmfIdentityContext(IServiceProvider sp)
    {
      this.sp = sp;
      this.crudOption = sp.GetService<CrudOption>();
      this.tenantConnection = sp.GetService<TenantConnection>();
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
      var handler = this.GetType().GetComponentsByHandler<IDbContextBuilder>().OrderBy(b => b.Index);
      foreach (var h in handler)
      {
        h.SingletonHandler<IDbContextBuilder>()?.OnModelCreating(builder, sp, this);
      }
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
    protected virtual void SetConnectionString(Tenant? tenant, bool? reasonly, bool defaultConnection = false)
    {
      var crudOption = this.crudOption ?? sp.GetService<CrudOption>();
      var tenantConnection = this.tenantConnection ?? sp.GetService<TenantConnection>();
      if (crudOption?.SQLType == Enums.EnumSQLType.Memory)
      {
        return;
      }
      if (defaultConnection == true)
      {
        goto UseDefaultConnection;
      }
      if (!String.IsNullOrEmpty(tenant?.MainConnectionString))
      {
        if (tenantConnection?.Items?.TryGetValueStringKey(tenant?.MainConnectionString!, out var connection) == true && connection != null)
        {
          if (reasonly != true)
          {
            if (String.IsNullOrEmpty(connection.ReadAndWrite))
            {
              goto UseDefaultConnection;
            }
            Database.SetConnectionString(connection.ReadAndWrite);
            return;
          }
          else
          {
            if (connection?.ReadOnlyList?.Any() != true)
            {
              if (!String.IsNullOrEmpty(connection?.ReadAndWrite))
              {
                Database.SetConnectionString(connection.ReadAndWrite);
                return;
              }
              goto UseDefaultConnection;
            }
            Database.SetConnectionString(connection.ReadOnlyList[DateTime.Now.Microsecond % connection.ReadOnlyList.Length]);
            return;
          }
        }
      }
    UseDefaultConnection:
      if (reasonly != true)
      {
        if (!String.IsNullOrEmpty(crudOption?.DefaultConnectionString))
        {
          Database.SetConnectionString(crudOption?.DefaultConnectionString);
          return;
        }
      }
      else
      {
        if (!String.IsNullOrEmpty(crudOption?.DefaultReadOnlyConnectionString))
        {
          Database.SetConnectionString(crudOption?.DefaultReadOnlyConnectionString);
          return;
        }
        else
        {
          if (!String.IsNullOrEmpty(crudOption?.DefaultConnectionString))
          {
            Database.SetConnectionString(crudOption?.DefaultConnectionString);
            return;
          }
        }
      }
    }
    public void UseDefaultConnection()
    {
      SetConnectionString(ThisTenant, ReadOnly, true);
    }
    public void UseTenantConnection()
    {
      SetConnectionString(ThisTenant, ReadOnly, false);
    }
    public void SetTenant(Tenant? tenant)
    {
      ThisTenant = tenant;
      SetConnectionString(ThisTenant, ReadOnly);
    }

    public void SetReadOnly(bool readOnly)
    {
      ReadOnly = readOnly;
      SetConnectionString(ThisTenant, ReadOnly);
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
