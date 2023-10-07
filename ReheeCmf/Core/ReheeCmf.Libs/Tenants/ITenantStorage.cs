using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Tenants
{
  public interface ITenantStorage
  {
    IEnumerable<TenantEntity> GetAllTenants();
    void AddOrUpdateTenant(TenantEntity tenant);
    void RemoveTenant(TenantEntity tenant);
    void ClearCashed();

  }

  public class TenantStorage : ITenantStorage
  {
    private static object Locker { get; set; } = new object();
    private readonly IContext context;
    private static Lazy<ConcurrentDictionary<Guid, TenantEntity>> _tenantCache =
      new Lazy<ConcurrentDictionary<Guid, TenantEntity>>(() => new ConcurrentDictionary<Guid, TenantEntity>());
    private static DateTime? lastUpdated = null;
    public TenantStorage(IContext context)
    {
      this.context = context;
    }
    public IEnumerable<TenantEntity> GetAllTenants()
    {
      if (lastUpdated == null)
      {
        lock (Locker)
        {
          var tenants = context.Query<TenantEntity>(true).ToArray();
          _tenantCache.Value.Clear();
          foreach (var t in tenants)
          {
            _tenantCache.Value.AddOrUpdate(t.Id, t, (b, c) => t);
          }
          lastUpdated = DateTime.UtcNow;
        }
      }
      return _tenantCache.Value.Values;
    }
    public void AddOrUpdateTenant(TenantEntity tenant)
    {
      _tenantCache.Value.AddOrUpdate(tenant.Id, tenant, (b, c) => tenant);

    }
    public void RemoveTenant(TenantEntity tenant)
    {
      _tenantCache.Value.TryRemove(tenant.Id, out _);
    }

    public void ClearCashed()
    {
      _tenantCache.Value.Clear();
      lastUpdated = null;
    }
  }
}
