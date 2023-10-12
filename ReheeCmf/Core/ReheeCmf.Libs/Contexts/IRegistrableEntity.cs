using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Contexts
{
  public interface IRegistrableEntity
  {
    void RegisterEntity(object builder, ITenantContext db);
    IEnumerable<Type> QueryableEntities { get; }
  }
}
