using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Tenants
{
  public interface IGetCurrentTenant
  {
    Guid? GetCurrentTenantId();
    string GetCurrentTenantName();
  }
}
