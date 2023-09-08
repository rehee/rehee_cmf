namespace ReheeCmf.Handlers.EntityChangeHandlers
{
  public abstract class EntityChangeHandler<T> : IEntityChangeHandler<T> where T : class
  {
    protected IServiceProvider? sp { get; set; }
    protected T? entity { get; set; }
    protected IContext? context { get; set; }
    protected IContextScope<TokenDTO>? scopedUser { get; set; }
    public void Init(IServiceProvider sp, object entity)
    {
      if (sp is T e)
      {
        InitWithType(sp, e);
      }

    }
    public void InitWithType(IServiceProvider sp, T entity)
    {
      this.sp = sp;
      this.entity = entity;
      this.context = sp.GetService<IContext>();
      this.scopedUser = sp.GetService<IContextScope<TokenDTO>>();
    }
    public void Dispose()
    {
      sp = null;
      entity = null;
      context = null;
      scopedUser = null;
    }
    public virtual Task SetTenant(CancellationToken ct = default)
    {
      if (entity is IWithTenant != true)
      {
        return Task.CompletedTask;
      }

      var service = sp!.GetService<IEntityTenantService<T>>();
      var tEntity = (entity as IWithTenant)!;
      if (service != null)
      {
        tEntity.TenantID = service.GetTenant(context, scopedUser?.Value);
        return Task.CompletedTask;
      }
      if (tEntity.TenantID == null)
      {
        tEntity.TenantID = context?.TenantID;
      }

      return Task.CompletedTask;
    }
    public virtual Task AfterCreateAsync(CancellationToken ct = default)
    {
      return Task.CompletedTask;
    }

    public virtual Task AfterDeleteAsync(CancellationToken ct = default)
    {
      return Task.CompletedTask;
    }

    public virtual Task AfterUpdateAsync(CancellationToken ct = default)
    {
      return Task.CompletedTask;
    }

    public virtual async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await SetTenant(ct);
    }

    public virtual Task BeforeDeleteAsync(CancellationToken ct = default)
    {
      return Task.CompletedTask;
    }

    public virtual Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default)
    {
      return Task.CompletedTask;
    }



    public virtual Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default)
    {
      return Task.FromResult(Enumerable.Empty<ValidationResult>());
    }
  }
}
