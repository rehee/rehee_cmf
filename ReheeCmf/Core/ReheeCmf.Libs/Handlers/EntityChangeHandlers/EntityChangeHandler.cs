using System;
using System.Net;

namespace ReheeCmf.Handlers.EntityChangeHandlers
{
  public abstract class EntityChangeHandler<T> : IEntityChangeHandler where T : class
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
    public EnumEntityChange Status { get; protected set; }



    public virtual void Init(IServiceProvider sp, object entity, int index, int subindex, string? group = null)
    {
      if (entity == null)
      {
        StatusException.Throw(ValidationResultHelper.New("Entity not be track", "entity"));
      }
      if (entity is T != true)
      {
        StatusException.Throw(ValidationResultHelper.New("Entity not be track", "entity"));
      }
      EntityHashCode = entity.GetHashCode();
      var typed = entity as T;
      this.sp = sp;
      this.entity = typed;
      this.context = sp.GetService<IContext>();
      this.scopedUser = sp.GetService<IContextScope<TokenDTO>>();
      this.scopedTenant = sp.GetService<IContextScope<Tenant>>();
      this.Index = index;
      this.SubIndex = subindex;
      this.Group = group;

      Status = EnumEntityChange.NoChanges;
    }
    public virtual void Dispose()
    {
      sp = null;
      entity = null;
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
      Status = EnumEntityChange.NoChanges;
      return Task.CompletedTask;
    }

    public virtual Task AfterDeleteAsync(CancellationToken ct = default)
    {
      Status = EnumEntityChange.NoChanges;
      return Task.CompletedTask;
    }

    public virtual Task AfterUpdateAsync(CancellationToken ct = default)
    {
      Status = EnumEntityChange.NoChanges;
      return Task.CompletedTask;
    }

    public virtual async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await SetTenant(ct);
      Status = EnumEntityChange.Create;
    }

    public virtual Task BeforeDeleteAsync(CancellationToken ct = default)
    {
      Status = EnumEntityChange.Delete;
      return Task.CompletedTask;

    }

    public virtual Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default)
    {
      Status = EnumEntityChange.Update;
      return Task.CompletedTask;
    }


    public virtual Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default)
    {
      return Task.FromResult(Enumerable.Empty<ValidationResult>());
    }

    public void Init(IServiceProvider sp, object entity)
    {
      throw new NotImplementedException();
    }

    public Task SetTeSubindexSubindexnant(CancellationToken ct = default)
    {
      throw new NotImplementedException();
    }
  }
}
