using ReheeCmf.Caches;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Helpers
{
  public static class CacheBaseHelper
  {
    public static ConcurrentDictionary<Type, IKeyValueCaches> KeyValueCacheMap { get; set; } = new ConcurrentDictionary<Type, IKeyValueCaches>();

    public static IKeyValueCaches? GetCache(Type type)
    {
      KeyValueCacheMap.TryGetValue(type, out var result);
      return result;
    }
  }
}
