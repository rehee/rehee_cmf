using ReheeCmf.Caches;
using ReheeCmf.Commons.Jsons.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReheeCmf.Storages
{
  public class Storage<T, TKey> : IStorage<T, TKey> where T : class, IId<TKey> where TKey : IEquatable<TKey>
  {
    protected readonly IContext context;
    protected readonly IKeyValueCaches<T> cache;
    protected readonly IAsyncQuery asyncQuery;
    private readonly IStorageLocker<T> locker;
    protected virtual string GetKey(TKey key)
    {
      return $"{context.TenantID?.ToString() ?? ""}_{key.ToString()}";
    }
    protected virtual Guid GetTenant()
    {
      return context?.TenantID ?? Guid.Empty;
    }
    public Storage(IContext context, IKeyValueCaches<T> cache, IAsyncQuery asyncQuery, IStorageLocker<T> locker)
    {
      this.context = context;
      this.cache = cache;
      this.asyncQuery = asyncQuery;
      this.locker = locker;
    }
    protected async Task ResetCache(CancellationToken ct)
    {
      if (!locker.CacheUpdate.TryGetValue(GetTenant(), out var updateTime) || updateTime == null)
      {
        var thisLocker = locker.AsyncLock.GetOrAdd(GetTenant(), b => new SemaphoreSlim(1, 1));
        var code = thisLocker.GetHashCode();
        await thisLocker.WaitAsync();
        try
        {
          var lists = await asyncQuery.ToArrayAsync(context.Query<T>(true), ct);
          if (lists != null)
          {
            await Task.WhenAll(
              lists.Select(item => cache.SetAsync(GetKey(item.Id), item, -1, ct)));
          }
          locker.CacheUpdate.TryAdd(GetTenant(), DateTime.UtcNow);
        }
        catch
        {

        }
        finally
        {
          thisLocker.Release();
        }
      }
    }
    public virtual async Task<T?> GetAsync(TKey id, CancellationToken ct)
    {
      await ResetCache(ct);
      return await cache.GetAsync<T>(GetKey(id), ct);
    }
    public virtual async Task<IQueryable<T>> GetQueryAsync(CancellationToken ct)
    {
      await ResetCache(ct);
      return cache.Query<T>();
    }
    public async virtual Task AfterCreatetOrUpdateAsync(T? entity, CancellationToken ct)
    {
      await ResetCache(ct);
      if (entity == null)
      {
        return;
      }
      var obj = context.Query<T>(true).Where(b => b.Id.Equals(entity.Id)).FirstOrDefault();
      await cache.SetAsync(GetKey(obj!.Id), obj, -1, ct);
    }

    public virtual async Task AfterDeletetAsync(T? entity, CancellationToken ct)
    {
      await ResetCache(ct);
      if (entity == null)
      {
        return;
      }
      await cache.RemoveAsync(GetKey(entity.Id), ct);
    }


  }
}
