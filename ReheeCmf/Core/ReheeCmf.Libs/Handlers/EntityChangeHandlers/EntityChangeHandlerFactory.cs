using Microsoft.VisualBasic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Handlers.EntityChangeHandlers
{
  public static class EntityChangeHandlerFactory
  {
    private static ConcurrentDictionary<int, IEntityChangeTrackerComponent> tracker = new ConcurrentDictionary<int, IEntityChangeTrackerComponent>();
    public static IEnumerable<IEntityChangeTrackerComponent> CreateHandler(Type type)
    {
      IEnumerable<IEntityChangeTrackerComponent> result = (type.IsInterface ?
          tracker.Values.Where(b => b.EntityType.IsImplement(type)) :
          tracker.Values.Where(b => b.NoInherit ? b.EntityType.Equals(type) : type.IsInheritance(b.EntityType)));
      if (result?.Any() != true)
      {
        return Enumerable.Empty<IEntityChangeTrackerComponent>();
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

    public static void Init()
    {
      AppDomain currentDomain = AppDomain.CurrentDomain;
      Assembly[] assemblies = currentDomain.GetAssemblies();

      foreach (var a in assemblies)
      {
        foreach (var b in
          a.GetTypes()
          .Where(b => b.CustomAttributes.Any(b => b.AttributeType.IsImplement<IEntityChangeTrackerComponent>()))
          .SelectMany(b => b.GetCustomAttributes()))
        {
          if (b is IEntityChangeTrackerComponent attr)
          {
            tracker.TryAdd(attr.GetHashCode(), attr);
          }
        }
      }
    }
  }
}
