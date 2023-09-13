using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReheeCmf.Commons.Interfaces;
using ReheeCmf.Handlers.ChangeHandlers;

namespace ReheeCmf.ContextModule.Contexts
{
  public class CmfRepositoryContext : IContext, ICrudTracker
  {
    protected readonly DbContext context;
    public object Context => context;
    protected readonly IServiceProvider sp;
    protected ConcurrentDictionary<int, IChangeHandler>? EntityChangeHandlerMapper { get; set; }
    public CmfRepositoryContext(IServiceProvider sp, DbContext context)
    {
      this.sp = sp;
      this.context = context;
      if (context is IWithContext ct)
      {
        ct.Context = this;
      }
      EntityChangeHandlerMapper = new ConcurrentDictionary<int, IChangeHandler>();
      scopeTenant = sp.GetService<IContextScope<Tenant>>()!;
      tenant = scopeTenant?.Value;
      scopeTenant!.ValueChange += ScopeTenant_ValueChange;

      scopeUser = sp.GetService<IContextScope<TokenDTO>>()!;
      user = scopeUser?.Value;
      scopeUser!.ValueChange += CmfRepositoryContext_ValueChange;

      if (context != null)
      {
        context.SavedChanges += CmfDbContextEvent.SavedChangesEventArgs!;
        context.ChangeTracker.Tracked += CmfDbContextEvent.ChangeTracker_StateChanged!;
        context.ChangeTracker.StateChanged += CmfDbContextEvent.ChangeTracker_StateChanged!;
        //context.ChangeTracker.StateChanging += ChangeTracker_StateChanging;
      }

    }

    private void ChangeTracker_StateChanging(object? sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityStateChangingEventArgs e)
    {
      if (e.Entry.Entity is IWithName nn)
      {
        nn.Name1 = Guid.NewGuid().ToString();
      }
    }

    private void ChangeTracker_DetectedEntityChanges(object? sender, Microsoft.EntityFrameworkCore.ChangeTracking.DetectedEntityChangesEventArgs e)
    {
      if (e.Entry.Entity is IWithName nn)
      {
        nn.Name1 = Guid.NewGuid().ToString();
      }
    }

    private void ChangeTracker_StateChanged(object? sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityStateChangedEventArgs e)
    {
      if (e.Entry.Entity is IWithName b)
      {
        b.Name1 = Guid.NewGuid().ToString();
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
        context.ChangeTracker.Tracked -= CmfDbContextEvent.ChangeTracker_StateChanged!;
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
      return this.GetMap().Methods.FirstOrDefault(b => b.Name.Equals(nameof(QueryWithType)))!
        .MakeGenericMethod(type).Invoke(this, new object[] { noTracking });
    }
    public IQueryable<T> QueryWithType<T>(bool asNoTracking) where T : class
    {
      return Query<T>(asNoTracking);
    }
    public object? Find(Type type, object key)
    {
      return this.GetMap().Methods.FirstOrDefault(b => b.Name.Equals(nameof(FindWithType)))!
        .MakeGenericMethod(type).Invoke(this, new object[] { key });
    }
    public T? FindWithType<T>(object key) where T : class
    {
      return context.Set<T>().Find(key);
    }
    public void Add(Type type, object? value)
    {
      this.GetMap().Methods.FirstOrDefault(b => b.Name.Equals(nameof(AddWithType)))!
        .MakeGenericMethod(type).Invoke(this, new object[] { value });
    }
    public void AddWithType<T>(object? value) where T : class
    {
      if (value == null || value is T != true)
      {
        return;
      }
      context.Set<T>().Add(value as T);
    }
    public void Delete(Type type, object key)
    {
      var entity = Find(type, key);
      if (entity == null)
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      this.GetMap().Methods.FirstOrDefault(b => b.Name.Equals(nameof(DeleteByType)))!
        .MakeGenericMethod(type).Invoke(this, new object[] { entity! });
    }
    public void DeleteByType<T>(object entity) where T : class
    {
      if (entity is T tEntity)
      {
        context.Remove<T>(tEntity);
        return;
      }
      StatusException.Throw(HttpStatusCode.NotFound);
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
      var components = ComponentFactory.CreateEntityChangeComponents(entityType);
      if (components?.Any() != true || EntityChangeHandlerMapper == null)
      {
        return;
      }
      foreach (var component in components)
      {
        var handlerKey = component.GetHashCode() + entity.GetHashCode();
        if (EntityChangeHandlerMapper.TryGetValue(handlerKey, out _))
        {
          continue;
        }
        var handler = component.CreateChangeHandler(sp, entity);
        EntityChangeHandlerMapper.TryAdd(handlerKey, handler);
      }

    }

    public IEnumerable<IChangeHandler> GetHandlers(object entity)
    {
      if (EntityChangeHandlerMapper == null)
      {
        return Enumerable.Empty<IEntityChangeHandler>();
      }
      var values = EntityChangeHandlerMapper.Values
        .Where(b => b.EntityHashCode == entity.GetHashCode()).OrderBy(b => b.Index).ThenBy(b => b.SubIndex);
      if (values?.Any() != true)
      {
        return Enumerable.Empty<IChangeHandler>();
      }
      return values
        .GroupBy(b => b.Group).Select(b => b.OrderBy(b => b.Index).ThenBy(b => b.SubIndex).AsEnumerable())
        .Aggregate((a, b) => a.Concat(b));
    }
  }
}
