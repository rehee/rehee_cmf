using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Caches.MemoryCaches
{
  public interface ICacheManager
  {
    IEnumerable<object> Keys { get; }
    ConcurrentDictionary<object, DateTimeOffset> KeyLastVisit { get; }
    int Count { get; }
    void CleanExpiredCache();
  }
}
