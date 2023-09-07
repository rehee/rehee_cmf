namespace ReheeCmf.Handlers.EntityChangeHandlers
{
  public interface IEntityChangeHandler : IValidationHandler, IDisposable
  {
    void Init(IServiceProvider sp, object entity);

    Task SetTenant(CancellationToken ct = default);

    Task BeforeCreateAsync(CancellationToken ct = default);
    Task AfterCreateAsync(CancellationToken ct = default);

    Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default);
    Task AfterUpdateAsync(CancellationToken ct = default);

    Task BeforeDeleteAsync(CancellationToken ct = default);
    Task AfterDeleteAsync(CancellationToken ct = default);

  }
  public interface IEntityChangeHandler<T> : IEntityChangeHandler where T : class
  {
    void InitWithType(IServiceProvider sp, T entity);
  }
}
