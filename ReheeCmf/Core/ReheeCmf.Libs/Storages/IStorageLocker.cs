using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Storages
{
  public interface IStorageLocker<T>
  {
    ConcurrentDictionary<Guid, SemaphoreSlim> AsyncLock { get; }
    ConcurrentDictionary<Guid, DateTime?> CacheUpdate { get; }
  }
}
