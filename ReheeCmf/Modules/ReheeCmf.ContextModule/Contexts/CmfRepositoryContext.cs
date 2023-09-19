using ReheeCmf.Commons.Interfaces;
using ReheeCmf.Enums;
using ReheeCmf.Handlers.ChangeHandlers;

namespace ReheeCmf.ContextModule.Contexts
{
  public class CmfRepositoryContext : IContext, ICrudTracker
  {
    protected readonly DbContext context;
    protected ITenantContext? TenantContext
    {
      get
      {
        if (context is ITenantContext tc)
        {
          return tc;
        }
        return null;
      }
    }
    protected ITokenDTOContext? TokenDTOContext
    {
      get
      {
        if (context is ITokenDTOContext tc)
        {
          return tc;
        }
        return null;
      }
    }
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

    public TokenDTO? User => TokenDTOContext?.User;
    public Tenant? ThisTenant => TenantContext?.ThisTenant;

    public bool IgnoreTenant => TenantContext?.IgnoreTenant ?? false;
    public void SetIgnoreTenant(bool ignore)
    {
      TenantContext?.SetIgnoreTenant(ignore);
    }
    public Guid? TenantID
    {
      get => TenantContext?.TenantID;
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
    public IEnumerable<KeyValueItemDTO> GetKeyValueItemDTO(Type type)
    {
      var result = this.GetMap().Methods.FirstOrDefault(b => b.Name.Equals(nameof(GetKeyValueItemDTOWithType)))!
        .MakeGenericMethod(type).Invoke(this, null);
      if (result != null && result is KeyValueItemDTO[] list)
      {
        return list;
      }
      return Enumerable.Empty<KeyValueItemDTO>();
    }
    public KeyValueItemDTO[] GetKeyValueItemDTOWithType<T>() where T : class, ISelect
    {
      var component = ComponentFactory.GetSelectEntityComponent(typeof(T));
      if (component != null)
      {
        var handler = component.GetSelectHandler();
        if (handler != null)
        {
          return handler!.GetSelectItem(this).ToArray();
        }
      }
      return context.Set<T>().AsNoTracking().Select(b =>
        new KeyValueItemDTO
        {
          Key = b.SelectKey,
          Value = b.SelectValue,
        }).ToArray();
    }
    public object? Query(Type type, bool noTracking, bool readCheck = false)
    {
      return this.GetMap().Methods.FirstOrDefault(b => b.Name.Equals(nameof(QueryWithType)))!
        .MakeGenericMethod(type).Invoke(this, new object[] { noTracking, readCheck });
    }
    public IQueryable<T> QueryWithType<T>(bool asNoTracking, bool readCheck = false) where T : class
    {
      if (!readCheck)
      {
        return Query<T>(asNoTracking);
      }
      return Query<T>(asNoTracking).WhereCheck<T>(User);
    }

    public object? QueryWithKey(Type type, Type keyType, bool noTracking, object key, bool readCheck = false)
    {
      return this.GetMap().Methods.FirstOrDefault(b => b.Name.Equals(nameof(QueryWithTypeAndKey)))!
       .MakeGenericMethod(type, keyType).Invoke(this, new object[] { noTracking, key, readCheck });
    }
    public IQueryable<T> QueryWithTypeAndKey<T, TKey>(bool asNoTracking, TKey key, bool readCheck = false)
      where T : class, IId<TKey>
      where TKey : IEquatable<TKey>
    {
      if (!readCheck)
      {
        return Query<T>(asNoTracking).Where(b => b.Id.Equals(key));
      }
      return Query<T>(asNoTracking).Where(b => b.Id.Equals(key)).WhereCheck(User);
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
      if (User == null)
      {
        SetUser(user);
      }
      return context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync(TokenDTO? user, CancellationToken ct = default)
    {
      if (User == null)
      {
        SetUser(user);
      }
      return await context.SaveChangesAsync(ct);
    }
    public void SetReadOnly(bool readOnly)
    {

    }
    public void SetTenant(Tenant tenant)
    {
      TenantContext?.SetTenant(tenant);
    }

    public async Task AfterSaveChangesAsync(CancellationToken ct = default)
    {
      if (EntityChangeHandlerMapper == null || EntityChangeHandlerMapper.Values.Any() == false)
      {
        return;
      }
      foreach (var h in EntityChangeHandlerMapper.Values)
      {
        switch (h.EntityState)
        {
          case EnumEntityState.Modified:
            await h.AfterUpdateAsync(ct);
            break;
          case EnumEntityState.Deleted:
            await h.AfterDeleteAsync(ct);
            break;
          case EnumEntityState.Added:
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
        .Where(b => b?.EntityHashCode == entity.GetHashCode()).OrderBy(b => b.Index).ThenBy(b => b.SubIndex);
      if (values?.Any() != true)
      {
        return Enumerable.Empty<IChangeHandler>();
      }
      return values
        .GroupBy(b => b.Group).Select(b => b.OrderBy(b => b.Index).ThenBy(b => b.SubIndex).AsEnumerable())
        .Aggregate((a, b) => a.Concat(b));
    }

    public void SetUser(TokenDTO? user)
    {
      TokenDTOContext?.SetUser(user);
    }
    public Guid? CrossTenantID => TenantContext?.CrossTenant?.TenantID;
    public Tenant? CrossTenant => TenantContext?.CrossTenant;
    public void SetCrossTenant(Tenant? tenant)
    {
      TenantContext?.SetCrossTenant(tenant);
    }

    public void TrackEntity(object entity, EnumEntityState enumEntityStatus = EnumEntityState.Modified)
    {
      try
      {
        var entry = this.context.Entry(entity);
        if (entry == null)
        {
          return;
        }
        switch (enumEntityStatus)
        {
          case EnumEntityState.Modified:
            entry.State = EntityState.Modified;
            break;
          case EnumEntityState.Added:
            entry.State = EntityState.Added;
            break;
          case EnumEntityState.Deleted:
            entry.State = EntityState.Deleted;
            break;
          case EnumEntityState.Unchanged:
            entry.State = EntityState.Unchanged;
            break;
        }
      }
      catch
      {

      }

    }
  }
}
