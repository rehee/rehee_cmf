using ReheeCmf.Components;

namespace ReheeCmf.Handlers.EntityChangeHandlers
{
  public interface IEntityChangeHandler : IValidationHandler, IDisposable, ICmfHandler
  {
    int EntityHashCode { get; }
    int Index { get; }
    int SubIndex { get; }
    string? Group { get; }

    EnumEntityChange Status { get; }

    void Init(IServiceProvider sp, object entity, int index, int subindex, string? group = null);

    Task SetTeSubindexSubindexnant(CancellationToken ct = default);

    Task BeforeCreateAsync(CancellationToken ct = default);
    Task AfterCreateAsync(CancellationToken ct = default);

    Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default);
    Task AfterUpdateAsync(CancellationToken ct = default);

    Task BeforeDeleteAsync(CancellationToken ct = default);
    Task AfterDeleteAsync(CancellationToken ct = default);

  }
}
