using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.MultiTenants
{
  public interface IServiceWithTenant : IDisposable, ISetTenantDetail
  {
    Tenant? CurrentTenant { get; }
    event EventHandler<EventArgs<Tenant>> TenantChange;
  }
}
