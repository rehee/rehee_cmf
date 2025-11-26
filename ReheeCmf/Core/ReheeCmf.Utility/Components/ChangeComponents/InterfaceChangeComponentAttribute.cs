using ReheeCmf.Handlers.InterfaceChangeHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Components.ChangeComponents
{
  public interface IInterfaceChangeComponent : IChangeComponent
  {
  }


  public class InterfaceChangeComponentAttribute<T> : ChangeComponentEntityAttribute<T>, IInterfaceChangeComponent where T : IInterfaceChangeHandler, new()
  {
    public override bool IsAvailable(Type type)
    {
      if (EntityType == null || !EntityType.IsInterface)
      {
        return false;
      }
      return type.IsImplement(EntityType);
    }
  }
  public class InterfaceChangeTrackerAttribute<TInterface> : ChangeComponentHandlerAttribute<TInterface>, IInterfaceChangeComponent
  {
    public override bool IsAvailable(Type type)
    {
      if (EntityType == null || !EntityType.IsInterface)
      {
        return false;
      }
      return type.IsImplement(EntityType);
    }
  }
}
