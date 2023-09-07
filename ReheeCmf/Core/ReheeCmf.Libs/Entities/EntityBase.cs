using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Entities
{
  public class EntityBase : IEntityBase
  {
    public Guid? TenantId { get; set; }
  }
  public class EntityBase<T> : EntityBase, IEntityBase<T> where T : IComparable
  {
    public virtual T Id { get; set; }
  }
}
