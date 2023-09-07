using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Tenants
{
  public interface ITenantService
  {
    Tenant GetTenant(string? name = null);
    Tenant GetTenantById(Guid? tenantId = null);
    IEnumerable<Tenant> GetAllTenant();
  }
}
