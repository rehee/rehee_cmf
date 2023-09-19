namespace ReheeCmf.Handlers.ChangeHandlers
{
  public abstract class ChangeHandler<T> : IChangeHandler
  {
    protected IServiceProvider? sp { get; set; }
    protected T? entity { get; set; }
    protected IContext? context { get; set; }
    protected IContextScope<TokenDTO>? scopedUser { get; set; }
    protected IContextScope<Tenant>? scopedTenant { get; set; }
    public int EntityHashCode { get; protected set; }

    public int Index { get; protected set; }
    public int SubIndex { get; protected set; }
    public string? Group { get; protected set; }
    public EnumEntityState EntityState { get; protected set; }

    public virtual void Init(IServiceProvider sp, object entity, int index, int subindex, string? group = null)
    {
      if (entity == null)
      {
        StatusException.Throw(ValidationResultHelper.New("Entity not be track", "entity"));
      }
      var types = typeof(T);
      if (entity is T != true)
      {
        StatusException.Throw(ValidationResultHelper.New("Entity not be track", "entity"));
      }

      EntityHashCode = entity!.GetHashCode();
      if (entity is T typed)
      {
        this.entity = typed;
      }
      this.sp = sp;
      this.context = sp.GetService<IContext>();
      this.scopedUser = sp.GetService<IContextScope<TokenDTO>>();
      this.scopedTenant = sp.GetService<IContextScope<Tenant>>();
      this.Index = index;
      this.SubIndex = subindex;
      this.Group = group;

      EntityState = EnumEntityState.Unchanged;
    }
    public virtual void Dispose()
    {
      sp = null;
      entity = default(T?);
      context = null;
      scopedUser = null;
      scopedTenant = null;
    }
    public virtual Task SetTenant(CancellationToken ct = default)
    {
      if (entity is IWithTenant != true)
      {
        return Task.CompletedTask;
      }
      return Task.CompletedTask;
    }
    public virtual Task AfterCreateAsync(CancellationToken ct = default)
    {
      EntityState = EnumEntityState.Unchanged;
      return Task.CompletedTask;
    }

    public virtual Task AfterDeleteAsync(CancellationToken ct = default)
    {
      EntityState = EnumEntityState.Unchanged;
      return Task.CompletedTask;
    }

    public virtual Task AfterUpdateAsync(CancellationToken ct = default)
    {
      EntityState = EnumEntityState.Unchanged;
      return Task.CompletedTask;
    }

    public virtual async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await SetTenant(ct);
      EntityState = EnumEntityState.Added;
    }

    public virtual Task BeforeDeleteAsync(CancellationToken ct = default)
    {
      EntityState = EnumEntityState.Deleted;
      return Task.CompletedTask;

    }

    public virtual Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default)
    {
      EntityState = EnumEntityState.Modified;
      return Task.CompletedTask;
    }


    public virtual Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default)
    {
      return Task.FromResult(SelfValidation());
    }

    protected virtual IEnumerable<ValidationResult> SelfValidation()
    {
      if (entity == null)
      {
        return Enumerable.Empty<ValidationResult>();
      }
      var result = new List<ValidationResult>();
      var context = new ValidationContext(entity);
      if (Validator.TryValidateObject(entity, context, result))
      {
        return Enumerable.Empty<ValidationResult>();
      }
      return result;
    }
  }
}
