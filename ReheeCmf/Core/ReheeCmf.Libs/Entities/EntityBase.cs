using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Entities
{
  public class EntityBase : IEntityBase, IWithTenant
  {
    public Guid? TenantID { get; set; }
  }
  public class EntityBase<T> : EntityBase, IEntityBase<T> where T : IEquatable<T>
  {
    public virtual T Id { get; set; }
  }
}
