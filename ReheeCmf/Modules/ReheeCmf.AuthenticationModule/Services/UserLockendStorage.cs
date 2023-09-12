using ReheeCmf.Commons;
using ReheeCmf.Services;
using ReheeCmf.Tenants;
using System.Collections.Concurrent;

namespace ReheeCmf.AuthenticationModule.Services
{
  public class UserLockendStorage : IUserLockendStorage
  {
    private readonly IContextScope<Tenant> tenant;
    private static ConcurrentDictionary<Guid, ConcurrentDictionary<string, (DateTimeOffset?, DateTimeOffset)>> userLockendMapper = new ConcurrentDictionary<Guid, ConcurrentDictionary<string, (DateTimeOffset?, DateTimeOffset)>>();
    private Guid TenantId { get; set; }
    public UserLockendStorage(IContextScope<Tenant> tenant)
    {
      this.tenant = tenant;
      if (tenant.Value?.TenantID.HasValue == true)
      {
        userLockendMapper.TryAdd(tenant.Value?.TenantID ?? Guid.Empty, new ConcurrentDictionary<string, (DateTimeOffset?, DateTimeOffset)>());
        TenantId = tenant.Value.TenantID.Value;
      }
      tenant.ValueChange += Tenant_ValueChange;
    }

    private void Tenant_ValueChange(object sender, ContextScopeEventArgs<Tenant> e)
    {
      if (e.Value?.TenantID.HasValue == true)
      {
        userLockendMapper.TryAdd(tenant.Value?.TenantID ?? Guid.Empty, new ConcurrentDictionary<string, (DateTimeOffset?, DateTimeOffset)>());
        TenantId = tenant.Value.TenantID.Value;
      }
    }
    public Task AddOrUpdateUserLockendAsync(string userName, DateTimeOffset? lockend, CancellationToken ct = default)
    {
      try
      {
        userName = userName.ToUpper();
        if (!userLockendMapper.TryGetValue(TenantId, out var mapper))
        {
          return Task.CompletedTask;
        }
        var expire = DateTimeOffset.UtcNow.AddMinutes(30);
        mapper.AddOrUpdate(userName, (lockend, expire), (a, b) => (lockend, expire));

      }
      catch
      {

      }
      return Task.CompletedTask;
    }

    public Task<(bool HasRecord, DateTimeOffset? Lockend)> GetUserLockendAsync(string userName, CancellationToken ct = default)
    {
      try
      {
        userName = userName.ToUpper();
        if (!userLockendMapper.TryGetValue(TenantId, out var mapper))
        {
          return Task.FromResult((false, default(DateTimeOffset?)));
        }
        if (!mapper.TryGetValue(userName, out var lockout))
        {
          return Task.FromResult((false, default(DateTimeOffset?)));
        }
        if (lockout.Item2 < DateTimeOffset.UtcNow)
        {
          mapper.TryRemove(userName, out var oldValue);
          return Task.FromResult((false, default(DateTimeOffset?)));
        }
        return Task.FromResult((true, lockout.Item1));
      }
      catch
      {
        return Task.FromResult((false, default(DateTimeOffset?)));
      }
    }


  }
}
