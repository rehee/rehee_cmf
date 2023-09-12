using ReheeCmf.Commons.DTOs;
using ReheeCmf.Commons;
using ReheeCmf.Services;
using ReheeCmf.Tenants;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.AuthenticationModule.Services
{
  public class UserTokenStorage : IUserTokenStorage
  {
    private readonly IContextScope<Tenant> tenant;
    private static ConcurrentDictionary<Guid, ConcurrentDictionary<string, TokenDTO>> userTokenMapper = new ConcurrentDictionary<Guid, ConcurrentDictionary<string, TokenDTO>>();
    private Guid TenantId { get; set; }
    public UserTokenStorage(IContextScope<Tenant> tenant)
    {
      this.tenant = tenant;
      if (tenant.Value?.TenantID.HasValue == true)
      {
        userTokenMapper.TryAdd(tenant.Value?.TenantID ?? Guid.Empty, new ConcurrentDictionary<string, TokenDTO>());
        TenantId = tenant.Value.TenantID.Value;
      }
      tenant.ValueChange += Tenant_ValueChange;
    }

    private void Tenant_ValueChange(object sender, ContextScopeEventArgs<Tenant> e)
    {
      if (e.Value?.TenantID.HasValue == true)
      {
        userTokenMapper.TryAdd(tenant.Value?.TenantID ?? Guid.Empty, new ConcurrentDictionary<string, TokenDTO>());
        TenantId = tenant.Value.TenantID.Value;
      }
    }

    public Task<bool> AddOrUpdateTokenAsync(string userName, TokenDTO token, CancellationToken ct = default)
    {
      try
      {
        userName = userName.ToUpper();
        if (userTokenMapper.TryGetValue(TenantId, out var mapper))
        {
          mapper.TryAdd(userName, token);
        }
        return Task.FromResult(true);
      }
      catch (Exception ex)
      {
        return Task.FromResult(false);
      }
    }
    public Task<TokenDTO?> GetUserTokenAsync(string userName, CancellationToken ct = default)
    {
      try
      {
        userName = userName.ToUpper();
        if (!userTokenMapper.TryGetValue(TenantId, out var mapper))
        {
          return Task.FromResult(default(TokenDTO?));
        }
        if (!mapper.TryGetValue(userName, out var token))
        {
          return Task.FromResult(default(TokenDTO?));
        }
        if (token.ExpireUCTTime < DateTime.UtcNow.AddMinutes(-2))
        {
          return Task.FromResult(token);
        }
        mapper.TryRemove(userName, out var oldDTO);
        return Task.FromResult(default(TokenDTO?));
      }
      catch (Exception ex)
      {
        return Task.FromResult(default(TokenDTO?));
      }
    }
  }
  
}
