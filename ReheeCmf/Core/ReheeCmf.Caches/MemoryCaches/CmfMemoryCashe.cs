using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Caches.MemoryCaches
{
  public class CmfMemoryCashe<T> : ITypedMemoryCache<T>
  {
    protected readonly IMemoryCache mc;
    protected readonly double keyExpireTime;
    protected readonly ConcurrentDictionary<object, DateTimeOffset> keyLastVisit;
    protected virtual bool enableTimeoutCheck => false;
    public CmfMemoryCashe(IMemoryCache mc, double? keyExpireTime = null, int? jobTime = null)
    {
      this.mc = mc;
      keyLastVisit = new ConcurrentDictionary<object, DateTimeOffset>();
      this.keyExpireTime = keyExpireTime ?? 1d;
      if (jobTime.HasValue && jobTime > 0)
      {
        Task.Run(async () =>
        {
          while (IsDispose == false)
          {
            await Task.Delay(1000 * 60 * jobTime.Value);
            CleanExpiredCache();
          }
        });
      }
    }


    public virtual ICacheEntry CreateEntry(object key)
    {
      if (keyExpireTime > 0)
      {
        keyLastVisit.TryAdd(key, DateTimeOffset.UtcNow.AddMinutes(keyExpireTime));
      }
      else
      {
        keyLastVisit.TryAdd(key, DateTimeOffset.UtcNow);
      }
      var entity = mc.CreateEntry(key);
      return entity;
    }
    public virtual void Remove(object key)
    {
      mc.Remove(key);
      keyLastVisit.TryRemove(key, out var deleteTime);
    }
    public virtual bool TryGetValue(object key, out object value)
    {
      var keyCheck = keyLastVisit.TryGetValue(key, out var valueCheck);
      if (!keyCheck)
      {
        value = null;
        return false;
      }
      if (keyExpireTime > 0 || enableTimeoutCheck)
      {
        if (valueCheck < DateTimeOffset.UtcNow)
        {
          value = null;
          Remove(key);
          return false;
        }
      }

      if (!mc.TryGetValue(key, out var valueInCache))
      {
        value = null;
        return false;
      }

      value = valueInCache;
      return true;
    }
    public bool IsDispose { get; protected set; }
    public virtual void CleanExpiredCache()
    {
      var current = DateTimeOffset.UtcNow;
      foreach (var key in keyLastVisit.Where(b => b.Value < current))
      {
        Remove(key.Key);
      }
    }
    public virtual void Dispose()
    {
      if (IsDispose)
      {
        return;
      }
      IsDispose = true;
      keyLastVisit.Clear();
      mc.Dispose();
    }

    protected Task CleanJob;
    public MemoryCacheStatistics? GetCurrentStatistics()
    {
      return mc.GetCurrentStatistics();
    }
  }

  public class CmfMemoryCashe<T, TKey, TValue> : CmfMemoryCashe<T>, ICmfCache<T, TKey, TValue>
  {
    public CmfMemoryCashe(IMemoryCache mc, double? keyExpireTime = null, int? jobTime = null) : base(mc, keyExpireTime, jobTime)
    {
    }

    public IEnumerable<object> Keys => KeyLastVisit.Keys;
    public int Count => Keys.Count();
    public ConcurrentDictionary<object, DateTimeOffset> KeyLastVisit => keyLastVisit;



    public async Task<bool> TryCreateOrUpdateValue(TKey key, TValue? value)
    {
      try
      {
        var tryGetValue = await TryGetValueAsync(key, out var existingValue);
        if (tryGetValue)
        {
          await RemoveAsync(key);
        }
        this.Set(key, value);
        return true;
      }
      catch
      {
        return false;
      }


    }

    public Task<bool> RemoveAsync(TKey key)
    {
      try
      {
        Remove(key);
        return Task.FromResult(true);
      }
      catch
      {
        return Task.FromResult(false);
      }
    }

    public Task<bool> TryGetValueAsync(TKey key, out TValue? value)
    {
      if (!TryGetValue(key, out var valueObj))
      {
        value = default;
        return Task.FromResult(false);
      }
      if (valueObj is TValue valueReturn)
      {
        value = valueReturn;
        return Task.FromResult(true);
      }
      value = default;
      return Task.FromResult(false);
    }
  }
}
