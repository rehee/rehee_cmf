using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Storages
{
  public class StorageLocker<T> : IStorageLocker<T>
  {
    public ConcurrentDictionary<Guid, SemaphoreSlim> AsyncLock { get; set; }

    public ConcurrentDictionary<Guid, DateTime?> CacheUpdate { get; set; }
    public StorageLocker()
    {
      AsyncLock = new ConcurrentDictionary<Guid, SemaphoreSlim>();
      CacheUpdate = new ConcurrentDictionary<Guid, DateTime?>();
    }
  }
}
