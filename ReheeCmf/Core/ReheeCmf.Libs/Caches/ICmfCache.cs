using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Caches.MemoryCaches
{
  public interface ICmfCache<T, TKey, TValue> : ICacheManager, IDisposable
  {
    Task<bool> TryGetValueAsync(TKey key, out TValue? value);
    Task<bool> TryCreateOrUpdateValue(TKey key, TValue? value);
    Task<bool> RemoveAsync(TKey key);
  }

}
