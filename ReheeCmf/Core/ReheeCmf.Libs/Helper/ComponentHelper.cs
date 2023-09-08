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
    public static IEnumerable<IComponentHandler> GetComponentHandler(this Type type)
    {
      return type.GetMap().Attributes.Select(b =>
      {
        if (b is IComponentHandler a)
        {
          return (true, a);
        }
        return (false, default(IComponentHandler));
      }).Where(b => b.Item1 == true).Select(b => b.Item2!);
    }
    public static IEnumerable<IComponentHandler> GetComponentHandler<T>(this Type type) where T : ICmfComponent
    {
      return type.GetComponentHandler().Where(b =>
      {
        return b.ComponentType.IsImplement<T>();
      });
    }
  }
}
