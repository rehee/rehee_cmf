using ReheeCmf.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Helper
{
  public static class ComponentHelper
  {

    public static IEnumerable<ICmfComponent> GetComponents(this Type type)
    {
      return type.GetMap().Attributes.Select(b =>
      {
        if (b is ICmfComponent a)
        {
          return (true, a);
        }
        return (false, default(ICmfComponent));
      }).Where(b => b.Item1 == true).Select(b => b.Item2!);
    }
    public static IEnumerable<TComponent> GetComponentByType<TComponent>(this Type type) where TComponent : ICmfComponent
    {
      return type.GetMap().Attributes.Select(b =>
      {
        if (b is TComponent a)
        {
          return (true, a);
        }
        return (false, default(TComponent));
      }).Where(b => b.Item1 == true).Select(b => b.Item2!);
    }
    public static IEnumerable<ICmfComponent> GetComponentsByHandler<THandler>(this Type type) where THandler : ICmfHandler
    {
      var handlerType = typeof(THandler);
      return type.GetComponents().Where(b =>
      {
        if (handlerType.IsInterface)
        {
          return b.HandlerType.IsImplement<THandler>();
        }
        return b.HandlerType.IsInheritance(handlerType);
      });
    }
  }
}
