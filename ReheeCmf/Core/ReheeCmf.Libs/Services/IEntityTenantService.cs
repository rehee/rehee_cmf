using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Services
{
  public interface IEntityTenantService<T> where T : class
  {
    Guid? GetTenant(IContext? context, TokenDTO? user);
  }
}
