using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Storages
{
  public interface IStorage<T, TKey> where T : class where TKey : IEquatable<TKey>
  {
    Task<T?> GetAsync(TKey id, CancellationToken ct);
    Task<IQueryable<T>> GetQueryAsync(CancellationToken ct);

    Task AfterCreatetOrUpdateAsync(T? entity, CancellationToken ct);
    Task AfterDeletetAsync(T? entity, CancellationToken ct);
  }
}
