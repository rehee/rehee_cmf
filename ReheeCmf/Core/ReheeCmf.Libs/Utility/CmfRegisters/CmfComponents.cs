using ReheeCmf.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Utility.CmfRegisters
{
  public static partial class CmfRegister
  {
    public static ConcurrentDictionary<int, ICmfComponent> ComponentPool =
      new ConcurrentDictionary<int, ICmfComponent>();
    public static ConcurrentDictionary<int, ICmfHandler> SingletonHandlerPool =
      new ConcurrentDictionary<int, ICmfHandler>();
    public static void RegisterComponent(Attribute attribute, Type decorate)
    {
      if (attribute is ICmfComponent != true)
      {
        return;
      }
      var component = attribute as ICmfComponent;
      if (component is IHandlerComponent)
      {
        component.HandlerType = decorate;
      }
      if (component is IEntityComponent)
      {
        component.EntityType = decorate;
      }
      ComponentPool.TryAdd(component!.GetHashCode(), component);

    }

  }
}
