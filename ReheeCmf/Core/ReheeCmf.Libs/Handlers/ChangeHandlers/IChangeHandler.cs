using ReheeCmf.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Handlers.ChangeHandlers
{
  public interface IChangeHandler : IValidationHandler, IDisposable, ICmfHandler
  {
    int EntityHashCode { get; }
    int Index { get; }
    int SubIndex { get; }
    string? Group { get; }

    EnumEntityChange Status { get; }

    void Init(IServiceProvider sp, object entity, int index, int subindex, string? group = null);
    Task BeforeCreateAsync(CancellationToken ct = default);
    Task AfterCreateAsync(CancellationToken ct = default);

    Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default);
    Task AfterUpdateAsync(CancellationToken ct = default);

    Task BeforeDeleteAsync(CancellationToken ct = default);
    Task AfterDeleteAsync(CancellationToken ct = default);
  }
}
