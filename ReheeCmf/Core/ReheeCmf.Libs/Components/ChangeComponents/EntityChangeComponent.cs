using ReheeCmf.Handlers.ChangeHandlerss;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Components.ChangeComponents
{
  public interface IEntityChangeComponent : IChangeComponent
  {
  }
  public class EntityChangeAttribute<T> : ChangeComponentAttribute<T>, IEntityChangeComponent where T : IEntityChangeHandler, new()
  {
    public override bool IsAvailable(Type type)
    {
      if (EntityType == null || EntityType.IsInterface)
      {
        return false;
      }
      return type.IsInterface ?
          EntityType.IsImplement(type) :
          NoInherit ? EntityType.Equals(type) : type.IsInheritance(EntityType);
    }
  }
  public class EntityChangeTrackerAttribute<TEntity, THandler> : EntityChangeAttribute<THandler>
    where TEntity : class
    where THandler : IEntityChangeHandler, new()
  {

    public override Type EntityType => typeof(TEntity);
    public override int GetHashCode()
    {
      unchecked
      {
        return base.GetHashCode() * 11 + EntityType.GetHashCode();
      }
    }
  }

}
