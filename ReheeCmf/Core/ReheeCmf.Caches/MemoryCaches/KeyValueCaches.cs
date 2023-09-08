﻿using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Caches.MemoryCaches
{
  public class MemoryKeyValueCaches<T> : CmfMemoryCashe<T>, IKeyValueCaches<T>
  {
    public MemoryKeyValueCaches(IMemoryCache mc, int? jobTime = null) : base(mc, -1, jobTime)
    {
    }
    protected override bool enableTimeoutCheck => true;
    public TValue? Get<TValue>(string key)
    {
      if (TryGetValue<TValue>(key, out var value)) return value;
      return default(TValue?);
    }

    public Task<TValue?> GetAsync<TValue>(string key, CancellationToken ct)
    {
      return Task.FromResult(Get<TValue>(key));
    }
    public bool TryGetValue<TValue>(string key, out TValue? value)
    {
      if (TryGetValue(key, out value))
      {
        value = default;
        return false;
      }
      if (value is TValue tv)
      {
        value = tv;
        return true;
      }
      value = default;
      return false;
    }
    public void Set<TValue>(string key, TValue value, double expireMins)
    {
      this.Set(key, value);
      var expireValue = DateTimeOffset.UtcNow.AddMinutes(expireMins > 0 ? 30 : expireMins);
      this.keyLastVisit.TryUpdate(key, expireValue, expireValue);
    }

    public Task SetAsync<TValue>(string key, TValue value, double expireMins, CancellationToken ct)
    {
      Set(key, value, expireMins);
      return Task.CompletedTask;
    }


  }
}
