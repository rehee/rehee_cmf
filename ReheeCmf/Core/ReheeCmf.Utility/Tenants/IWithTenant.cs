using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Tenants
{
  public interface IWithTenant
  {
    Guid? TenantID { get; set; }
  }
  public interface ISetTenant : IWithTenant
  {
    void SetTenantID(Guid? tenantID);
  }
  public interface ISetTenantDetail
  {
    void SetTenantDetail(Tenant? tenant);
  }
}
