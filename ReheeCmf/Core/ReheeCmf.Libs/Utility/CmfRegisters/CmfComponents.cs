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

    public static void RegisterComponent(Attribute attribute)
    {
      if (attribute is ICmfComponent != true)
      {
        return;
      }
      var component = attribute as ICmfComponent;
      ComponentPool.TryAdd(component!.GetHashCode(), component);

    }

  }
}
