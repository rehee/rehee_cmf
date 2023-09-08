namespace ReheeCmf.ContextModule.Contexts
{
  public class CmfRepositoryContext : IContext
  {
    public readonly DbContext Context;
    private readonly IServiceProvider sp;

    public CmfRepositoryContext(IServiceProvider sp, DbContext context)
    {
      this.sp = sp;
      this.Context = context;
      if (context is IWithContext ct)
      {
        ct.Context = this;
      }
      scopeTenant = sp.GetService<IContextScope<Tenant>>()!;
      tenant = scopeTenant?.Value;
      scopeTenant!.ValueChange += ScopeTenant_ValueChange;

      scopeUser = sp.GetService<IContextScope<TokenDTO>>()!;
      user = scopeUser?.Value;
      scopeUser!.ValueChange += CmfRepositoryContext_ValueChange;
    }
    bool IsDispose { get; set; }
    public void Dispose()
    {
      if (IsDispose)
      {
        return;
      }
      IsDispose = true;
      scopeTenant!.ValueChange -= ScopeTenant_ValueChange;
      tenant = null;
      scopeUser!.ValueChange -= CmfRepositoryContext_ValueChange;
      user = null;
      if (Context != null && Context is IWithContext ct)
      {
        Context.Dispose();
        if (ct.Context != null)
        {
          ct.Context = null;
        }
      }
    }

    private void CmfRepositoryContext_ValueChange(object? sender, ContextScopeEventArgs<TokenDTO> e)
    {
      user = e.Value;
    }
    private void ScopeTenant_ValueChange(object? sender, ContextScopeEventArgs<Tenant> e)
    {
      tenant = e.Value;
    }

    protected Tenant? tenant { get; set; }
    private readonly IContextScope<Tenant> scopeTenant;
    protected TokenDTO? user { get; set; }
    private readonly IContextScope<TokenDTO> scopeUser;

    public TokenDTO? User => user;

    public Tenant? ThisTenant => tenant;

    public bool IgnoreTenant { get; protected set; }
    public void SetIgnoreTenant(bool ignore)
    {
      IgnoreTenant = true;
    }
    public Guid? TenantID
    {
      get => tenant?.TenantID;
      set { }
    }

    public async Task AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class
    {
      await Context.Set<T>().AddAsync(entity, cancellationToken);
    }

    public void Delete<T>(T entity) where T : class
    {
      Context.Set<T>().Remove(entity);
    }

    public void Delete(object entity)
    {
      Context.Remove(entity);
    }

    public async Task ExecuteTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken)
    {
      var strategy = Context.Database.CreateExecutionStrategy();

      await strategy.ExecuteAsync(async (ct) =>
      {
        using var transaction = Context.Database.BeginTransaction();
        await action(ct);
        transaction.Commit();
      }, cancellationToken);
    }

    public async Task<T?> GetByIdAsync<T>(object id, CancellationToken cancellationToken) where T : class
    {
      var set = Context.Set<T>();
      return await set.FindAsync(new object[] { id }, cancellationToken);
    }

    public IQueryable<T> Query<T>(bool asNoTracking) where T : class
    {
      if (asNoTracking)
      {
        return Context.Set<T>().AsNoTracking();
      }
      return Context.Set<T>();
    }

    public int SaveChanges(TokenDTO? user)
    {
      return Context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync(TokenDTO? user, CancellationToken ct = default)
    {
      return await Context.SaveChangesAsync(ct);
    }
    public void SetReadOnly(bool readOnly)
    {

    }
    public void SetTenant(Tenant tenant)
    {
      this.tenant = tenant;
    }


  }
}
