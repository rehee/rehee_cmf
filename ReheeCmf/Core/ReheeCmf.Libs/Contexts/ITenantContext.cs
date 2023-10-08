using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Contexts
{
  public interface ITenantContext : IWithTenant, ICrossTenant
  {
    Tenant? ThisTenant { get; }
    void SetTenant(Tenant tenant);
    Boolean? ReadOnly { get; }
    void SetReadOnly(bool readOnly);
    bool IgnoreTenant { get; }
    void SetIgnoreTenant(bool ignore);
    void UseDefaultConnection();
    void UseTenantConnection();
  }
  public interface ICrossTenant
  {
    Guid? CrossTenantID { get; }
    Tenant? CrossTenant { get; }
    void SetCrossTenant(Tenant? tenant);
  }
}
