using ReheeCmf.FileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ReheeCmf.Servers.Services
{
  public class TenantService : ITenantService
  {
    private readonly IGetCurrentTenant getCurrentTenant;
    private readonly ITenantStorage storage;
    private readonly FileServiceOption fileOption;

    private Tenant? tenant { get; set; }
    public TenantService(IGetCurrentTenant getCurrentTenant, ITenantStorage storage, FileServiceOption fileOption)
    {
      this.getCurrentTenant = getCurrentTenant;
      this.storage = storage;
      this.fileOption = fileOption;
    }
    public Tenant? GetTenant(string? name = null)
    {
      if (tenant != null)
      {
        return tenant;
      }
      var currentId = getCurrentTenant.GetCurrentTenantId();
      var currentName = getCurrentTenant.GetCurrentTenantName();
      if (currentId != null)
      {
        tenant = GetAllTenant().Where(b => b.TenantID == currentId).FirstOrDefault();
        return tenant;
      }
      var nameCheck = name ?? currentName;
      if (!string.IsNullOrEmpty(nameCheck))
      {
        tenant = GetAllTenant().Where(b =>
          nameCheck.Equals(b.TenantSUbDomain, StringComparison.OrdinalIgnoreCase)
        ).FirstOrDefault();
        return tenant;
      }
      return null;
    }
    public Tenant? GetTenantById(Guid? tenantId = null)
    {
      if (tenant != null)
      {
        return tenant;
      }
      if (tenantId != null)
      {
        tenant = GetAllTenant().Where(b => b.TenantID == tenantId).FirstOrDefault();
        return tenant;
      }
      return GetTenant();
    }
    public IEnumerable<Tenant> GetAllTenant()
    {
      var current = DateTime.UtcNow;
      return storage.GetAllTenants().Where(b => !b.LicenceEnd.HasValue || b.LicenceEnd >= current).Select(b => b.GetTenant(fileOption));
    }


  }
}
