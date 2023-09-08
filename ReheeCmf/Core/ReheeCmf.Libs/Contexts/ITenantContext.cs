using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Contexts
{
  public interface ITenantContext : IWithTenant
  {
    Tenant? ThisTenant { get; }
    void SetTenant(Tenant tenant);
    void SetReadOnly(bool readOnly);
    bool IgnoreTenant { get; }
    void SetIgnoreTenant(bool ignore);
  }
}
