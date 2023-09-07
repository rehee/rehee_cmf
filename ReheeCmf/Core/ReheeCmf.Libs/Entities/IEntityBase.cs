using ReheeCmf.Commons;
using ReheeCmf.Tenants;

namespace ReheeCmf.Entities
{
  public interface IEntityBase : IWithTenant
  {

  }
  public interface IEntityBase<T> : IEntityBase, IId<T> where T : IComparable
  {

  }
}
