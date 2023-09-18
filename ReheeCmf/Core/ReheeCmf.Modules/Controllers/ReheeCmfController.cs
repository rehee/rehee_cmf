using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Authenticates;
using ReheeCmf.Caches;
using ReheeCmf.Commons.DTOs;

namespace ReheeCmf.Modules.Controllers
{
  public abstract class ReheeCmfController : Controller, IDisposable, IAsyncDisposable
  {
    public ReheeCmfController(IServiceProvider sp)
    {
      this.serviceProvider = sp;
      this.context = sp.GetService<IContext>();
      this.authorize = sp.GetService<IAuthorize>();
      this.crudOption = sp.GetService<CrudOption>()!;
      this.tenantDetail = sp.GetService<IContextScope<Tenant>>()!;
      this.scopeUser = sp.GetService<IContextScope<TokenDTO>>()!;
      this.queryMemoryCache = sp.GetService<IContextScope<QuerySecondCache>>()!;

      currentUser = scopeUser.Value;
      scopeUser.ValueChange += ScopeUser_ValueChange;
      currentTenant = tenantDetail.Value;
      tenantDetail.ValueChange += TenantDetail_ValueChange;
    }

    private void TenantDetail_ValueChange(object? sender, ContextScopeEventArgs<Tenant> e)
    {
      currentTenant = e.Value;
    }

    private void ScopeUser_ValueChange(object? sender, ContextScopeEventArgs<TokenDTO> e)
    {
      currentUser = e.Value;
    }

    public virtual bool FlagOnly => false;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IContext? context;
    protected readonly IAuthorize? authorize;
    protected readonly CrudOption crudOption;
    protected readonly IContextScope<Tenant> tenantDetail;
    protected readonly IContextScope<TokenDTO> scopeUser;
    protected readonly IContextScope<QuerySecondCache> queryMemoryCache;
    protected Tenant? currentTenant { get; set; }
    protected TokenDTO? currentUser { get; set; }



    #region dispose method
    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      currentUser = null;
      scopeUser.ValueChange -= ScopeUser_ValueChange;
      currentTenant = null;
      tenantDetail.ValueChange -= TenantDetail_ValueChange;
    }
    [NonAction]
    public ValueTask DisposeAsync()
    {
      Dispose(true);
      return ValueTask.CompletedTask;
    }
    #endregion

  }
}
