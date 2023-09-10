using ReheeCmf.ContextModule.Events;
using ReheeCmf.Handlers.EntityChangeHandlers;
using ReheeCmf.Helper;
using System.Collections.Concurrent;

namespace ReheeCmf.ContextModule.Contexts
{
  public class CmfRepositoryContext : IContext, ICrudTracker
  {
    protected readonly DbContext context;
    public object Context => context;
    protected readonly IServiceProvider sp;
    protected ConcurrentDictionary<int, IEntityChangeHandler>? EntityChangeHandlerMapper { get; set; }
    public CmfRepositoryContext(IServiceProvider sp, DbContext context)
    {
      this.sp = sp;
      this.context = context;
      if (context is IWithContext ct)
      {
        ct.Context = this;
      }
      EntityChangeHandlerMapper = new ConcurrentDictionary<int, IEntityChangeHandler>();
      scopeTenant = sp.GetService<IContextScope<Tenant>>()!;
      tenant = scopeTenant?.Value;
      scopeTenant!.ValueChange += ScopeTenant_ValueChange;

      scopeUser = sp.GetService<IContextScope<TokenDTO>>()!;
      user = scopeUser?.Value;
      scopeUser!.ValueChange += CmfRepositoryContext_ValueChange;

      if (context != null)
      {
        context.SavedChanges += CmfDbContextEvent.SavedChangesEventArgs!;
        context.ChangeTracker.Tracked += CmfDbContextEvent.ChangeTracker_Tracked!;
        context.ChangeTracker.StateChanged += CmfDbContextEvent.ChangeTracker_StateChanged!;
      }

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
      if (EntityChangeHandlerMapper != null)
      {
        foreach (var v in EntityChangeHandlerMapper.Values)
        {
          v.Dispose();
        }
        EntityChangeHandlerMapper.Clear();
        EntityChangeHandlerMapper = null;
      }
      if (context != null && context is IWithContext ct)
      {
        context.SavedChanges -= CmfDbContextEvent.SavedChangesEventArgs!;
        context.ChangeTracker.Tracked -= CmfDbContextEvent.ChangeTracker_Tracked!;
        context.ChangeTracker.StateChanged -= CmfDbContextEvent.ChangeTracker_StateChanged!;
        context.Dispose();
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
      await context.Set<T>().AddAsync(entity, cancellationToken);
    }

    public void Delete<T>(T entity) where T : class
    {
      context.Set<T>().Remove(entity);
    }

    public void Delete(object entity)
    {
      context.Remove(entity);
    }

    public async Task ExecuteTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken)
    {
      var strategy = context.Database.CreateExecutionStrategy();

      await strategy.ExecuteAsync(async (ct) =>
      {
        using var transaction = context.Database.BeginTransaction();
        await action(ct);
        transaction.Commit();
      }, cancellationToken);
    }

    public async Task<T?> GetByIdAsync<T>(object id, CancellationToken cancellationToken) where T : class
    {
      var set = context.Set<T>();
      return await set.FindAsync(new object[] { id }, cancellationToken);
    }

    public IQueryable<T> Query<T>(bool asNoTracking) where T : class
    {
      if (asNoTracking)
      {
        return context.Set<T>().AsNoTracking();
      }
      return context.Set<T>();
    }
    public object? Query(Type type, bool noTracking)
    {
      return this.GetMap().Methods.FirstOrDefault(b => b.Name.Equals(nameof(QueryWithType)))
        .MakeGenericMethod(type).Invoke(this, new object[] { noTracking });
    }
    public IQueryable<T> QueryWithType<T>(bool asNoTracking) where T : class
    {
      return Query<T>(asNoTracking);
    }

    public int SaveChanges(TokenDTO? user)
    {
      return context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync(TokenDTO? user, CancellationToken ct = default)
    {
      return await context.SaveChangesAsync(ct);
    }
    public void SetReadOnly(bool readOnly)
    {

    }
    public void SetTenant(Tenant tenant)
    {
      this.tenant = tenant;
    }

    public async Task AfterSaveChangesAsync(CancellationToken ct = default)
    {
      if (EntityChangeHandlerMapper == null || EntityChangeHandlerMapper.Values.Any() == false)
      {
        return;
      }
      foreach (var h in EntityChangeHandlerMapper.Values)
      {
        switch (h.Status)
        {
          case Enums.EnumEntityChange.Update:
            await h.AfterUpdateAsync(ct);
            break;
          case Enums.EnumEntityChange.Delete:
            await h.AfterDeleteAsync(ct);
            break;
          case Enums.EnumEntityChange.Create:
            await h.AfterCreateAsync(ct);
            break;
        }
      }
    }

    public void AddingTracker(Type entityType, object entity)
    {
      var components = EntityChangeHandlerFactory.CreateHandler(entityType);
      if (components?.Any() != true || EntityChangeHandlerMapper == null)
      {
        return;
      }
      foreach (var component in components)
      {
        var handler = component.CreateEntityChangeHandler(sp, entity);
        EntityChangeHandlerMapper.TryAdd(handler.GetHashCode(), handler);
      }

    }

    public IEnumerable<IEntityChangeHandler> GetHandlers(object entity)
    {
      if (EntityChangeHandlerMapper == null)
      {
        return Enumerable.Empty<IEntityChangeHandler>();
      }
      var values = EntityChangeHandlerMapper.Values
        .Where(b => b.EntityHashCode == entity.GetHashCode()).OrderBy(b => b.Index).ThenBy(b => b.SubIndex);
      if (values?.Any() != true)
      {
        return Enumerable.Empty<IEntityChangeHandler>();
      }
      return values
        .GroupBy(b => b.Group).Select(b => b.OrderBy(b => b.Index).ThenBy(b => b.SubIndex).AsEnumerable())
        .Aggregate((a, b) => a.Concat(b));
    }
  }
}
