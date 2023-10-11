using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Caches;
using ReheeCmf.Caches.MemoryCaches;

namespace ReheeCmf.Helpers
{
  public static class CmfMemoryCasheHelper
  {
    public static IServiceCollection AddCmfMemoryCache<T, K, V>(this IServiceCollection services, double? keyExpired = null, int? cleanJob = null)
    {
      var memorycache = new MemoryCache(new MemoryCacheOptions());
      var cmfMemoryCache = new CmfMemoryCashe<T, K, V>(memorycache, keyExpired, cleanJob);
      services.AddSingleton<ITypedMemoryCache<T>>(sp => cmfMemoryCache);
      services.AddSingleton<ICmfCache<T, K, V>>(sp => cmfMemoryCache);
      return services;
    }
    public static IServiceCollection AddMemoryKeyValueCache<T>(this IServiceCollection services, int? cleanJob = null)
    {
      var memorycache = new MemoryCache(new MemoryCacheOptions());
      var cmfMemoryCache = new MemoryKeyValueCaches<T>(memorycache, cleanJob);
      services.AddSingleton<IKeyValueCaches<T>>(sp => cmfMemoryCache);
      CacheBaseHelper.KeyValueCacheMap.AddOrUpdate(typeof(T), cmfMemoryCache, (t, v) => cmfMemoryCache);
      return services;
    }
    public static IServiceCollection AddMemoryKeyValueCacheByType(this IServiceCollection services, Type type, int? cleanJob = null)
    {
      var method = typeof(CmfMemoryCasheHelper)!.GetMethod("AddMemoryKeyValueCache")!.MakeGenericMethod(type);
      method.Invoke(null, [services, cleanJob]);
      return services;
    }
    public static IKeyValueCaches<T>? GetMemoryKeyValueCache<T>(this IServiceProvider sp)
    {
      return sp.GetService<IKeyValueCaches<T>>();
    }
    public static IKeyValueCaches? GetMemoryKeyValueCacheByType(this IServiceProvider sp, Type type)
    {
      var method = typeof(CmfMemoryCasheHelper)!.GetMethod("GetMemoryKeyValueCache")!.MakeGenericMethod(type);
      var result = method.Invoke(null, [sp]);
      if (result is IKeyValueCaches typeResult)
      {
        return typeResult;
      }
      return null;
    }
  }
}
