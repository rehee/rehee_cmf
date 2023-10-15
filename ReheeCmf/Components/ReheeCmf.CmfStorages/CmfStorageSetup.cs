using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Caches;
using ReheeCmf.Caches.MemoryCaches;
using ReheeCmf.Commons;
using ReheeCmf.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf
{
  public static class CmfStorageSetup
  {
    public static IServiceCollection AddCmfStorageSetup<T, TKey>(this IServiceCollection services) where T : class, IId<TKey> where TKey : IEquatable<TKey>
    {
      var memorycache = new MemoryCache(new MemoryCacheOptions());
      var cmfMemoryCache = new MemoryKeyValueCaches<T>(memorycache, -1);
      services.AddSingleton<IKeyValueCaches<T>>(cmfMemoryCache);
      services.AddSingleton<IStorageLocker<T>, StorageLocker<T>>();
      services.AddScoped<IStorage<T, TKey>, Storage<T, TKey>>();
      return services;
    }
  }
}
