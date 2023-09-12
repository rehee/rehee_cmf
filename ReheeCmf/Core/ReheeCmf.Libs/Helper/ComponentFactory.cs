using ReheeCmf.Components;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.Utility.CmfRegisters;

namespace ReheeCmf.Helper
{
  public static class ComponentFactory
  {
    public static IEnumerable<TComponent> GetComponent<TComponent>() where TComponent : ICmfComponent
    {
      return CmfRegister.ComponentPool.Values.Where(b => b is TComponent).Select(b =>
      {
        if (b is TComponent component)
        {
          return component;
        }
        return default(TComponent)!;
      });
    }
    public static IEnumerable<IChangeComponent> CreateEntityChangeComponents(Type type)
    {
      var component = GetComponent<IChangeComponent>().ToArray();

      IEnumerable<IChangeComponent> result = (type.IsInterface ?
          component.Where(b => b.EntityType?.IsImplement(type) ?? false) :
          component.Where(b => b.NoInherit ? b.EntityType?.Equals(type) ?? false : b.EntityType != null && type.IsInheritance(b.EntityType)));
      if (result?.Any() != true)
      {
        return Enumerable.Empty<IChangeComponent>();
      }
      var groups = result.GroupBy(b => b.Group).Select(b =>
      {
        var sub = b.OrderBy(b => b.Index).ThenBy(b => b.SubIndex).ToList();
        if (sub.Any(b => b.Unique))
        {
          return sub.Where(b => b.Unique).Take(0);
        }
        var skip = sub.FindIndex(b => b.SkipFollowing);
        if (skip > 0)
        {
          return sub.Take(skip);
        }
        return sub;
      });
      return groups.Aggregate((f, s) => f.Concat(s));
    }
  }
}
