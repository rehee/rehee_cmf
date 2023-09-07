using ReheeCmf.Modules;

namespace ReheeCmf.Helpers
{
  public static class ModuleHelper
  {
    public static IEnumerable<Assembly> BlizorAssemblies { get; set; } = Enumerable.Empty<Assembly>();
    public static ModuleDependOn GetDepend<T>(params Type[] types)
    {
      return new ModuleDependOn
      {
        DependType = typeof(T),
        GenericType = types?.Any() == true ? types.Select(b => b).ToArray() : new Type[] { }
      };
    }
    public static IEnumerable<ModuleDependOn> Depends(params ModuleDependOn[] types)
    {
      return types.ToArray();
    }


    public static IEnumerable<ServiceModule> GetAllService(ServiceModule mainModule, List<(Type, ServiceModule, int)> existingList = null, int level = 0)
    {

      var firstCall = level == 0;
      existingList = firstCall ? new List<(Type, ServiceModule, int)>() : existingList;
      existingList.Add((mainModule.GetType(), mainModule, level));
      var nextLevel = level + 1;
      foreach (var depend in mainModule.Depends())
      {
        var obj = Activator.CreateInstance(depend.DependType);
        if (obj is ServiceModule sm)
        {
          sm.SetGenericTypeParameters(depend.GenericType);
          GetAllService(obj as ServiceModule, existingList, nextLevel);
        }
      }
      if (!firstCall)
      {
        return null;
      }
      var group = existingList.GroupBy(b => b.Item1).Select(b =>
        (b.Key,
        b.OrderByDescending(c => c.Item3).Select(c => c.Item2).FirstOrDefault(),
        b.OrderByDescending(c => c.Item3).Select(c => c.Item3).FirstOrDefault()
        )).OrderByDescending(b => b.Item3).ToArray();
      var emptyList = group.Where(b => b.Item2 == null).Select(b => b.Key).ToArray();
      if (emptyList.Length > 0)
      {
        StatusException.Throw(System.Net.HttpStatusCode.NotImplemented, $"Depend module need parameter {String.Join(",", emptyList.Select(b => b.Name).ToArray())}");
      }
      return group.Select(b => b.Item2).ToArray();
    }

  }
}
