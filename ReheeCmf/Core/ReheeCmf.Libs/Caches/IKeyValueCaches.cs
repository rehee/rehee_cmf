using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Caches
{
  public interface IKeyValueCaches
  {
    TValue? Get<TValue>(string key);
    Task<TValue?> GetAsync<TValue>(string key, CancellationToken ct);
    bool TryGetValue<TValue>(string key, out TValue? value);
    void Set<TValue>(string key, TValue value, double expireMins);
    Task SetAsync<TValue>(string key, TValue value, double expireMins, CancellationToken ct);
    Task RemoveAsync(string key, CancellationToken ct);
    IQueryable<TValue> Query<TValue>();
  }
  public interface IKeyValueCaches<T> : IKeyValueCaches
  {

  }
}
