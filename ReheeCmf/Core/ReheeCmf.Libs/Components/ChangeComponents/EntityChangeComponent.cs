using ReheeCmf.Handlers.ChangeHandlers;
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

  public class EntityChangeAttribute<T> : ChangeComponentEntityAttribute<T>, IEntityChangeComponent where T : IEntityChangeHandler, new()
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

  public class EntityChangeTrackerAttribute<TEntity> : ChangeComponentHandlerAttribute<TEntity>, IEntityChangeComponent
    where TEntity : class
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

}
