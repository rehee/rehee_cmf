using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Servers.Services
{
  public class SettingTenantStorage : ITenantStorage
  {
    private readonly TenantSetting setting;

    public SettingTenantStorage(TenantSetting setting)
    {
      this.setting = setting;
    }

    public void AddOrUpdateTenant(TenantEntity tenant)
    {
      
    }

    public void ClearCashed()
    {
      
    }

    public IEnumerable<TenantEntity> GetAllTenants()
    {
      return setting.Tenants;
    }

    public void RemoveTenant(TenantEntity tenant)
    {
      
    }
  }
}
