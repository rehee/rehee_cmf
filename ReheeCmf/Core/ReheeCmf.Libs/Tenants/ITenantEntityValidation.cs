using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Tenants
{
  public interface ITenantEntityValidation
  {
    Task<IEnumerable<ValidationResult>> ValidateAsync(TenantEntity tenant, IContext context, CancellationToken ct = default);
  }
}
