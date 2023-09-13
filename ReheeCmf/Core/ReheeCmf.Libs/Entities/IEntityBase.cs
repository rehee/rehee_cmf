using ReheeCmf.Commons;
using ReheeCmf.Tenants;

namespace ReheeCmf.Entities
{
  public interface IEntityBase : IWithTenant, IStringId
  {

  }
  public interface IEntityBase<T> : IEntityBase, IId<T> where T : IEquatable<T>
  {

  }
  public interface IStringId
  {
    string StringId();
  }
}
