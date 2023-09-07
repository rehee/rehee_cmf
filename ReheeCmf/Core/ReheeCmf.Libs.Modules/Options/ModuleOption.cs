using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Modules.Options
{
  public class ModuleOption
  {
    public static ConcurrentDictionary<string, Type> ModuleMap { get; set; } = new ConcurrentDictionary<string, Type>();
    public static ConcurrentDictionary<string, ServiceModule> ExtureModuleMap { get; set; } = new ConcurrentDictionary<string, ServiceModule>();
    public static ConcurrentDictionary<string, Type> ExtureEntityMap { get; set; } = new ConcurrentDictionary<string, Type>();

    public static ConcurrentDictionary<string, object> ServiceOnModelCreatingMap { get; set; } = new ConcurrentDictionary<string, object>();
    public static ConcurrentDictionary<string, object> ServiceOnConfiguringMap { get; set; } = new ConcurrentDictionary<string, object>();
    public static ConcurrentDictionary<string, (Type, string)> EntityInContextMap { get; set; } = new ConcurrentDictionary<string, (Type, string)>();

    public static ConcurrentDictionary<string, Type> ModuleAssemblyMap { get; set; } = new ConcurrentDictionary<string, Type>();
    public static IEnumerable<NavActionItemDTO> ModuleMenus { get; set; } = Enumerable.Empty<NavActionItemDTO>();

    public static void SetUpModule<T>(string moduleName, bool isAssembly = false) where T : ServiceModule
    {
      SetUpModule(moduleName, typeof(T), isAssembly);
    }
    public static void SetUpModule(string moduleName, Type type, bool isAssembly = false)
    {
      var name = moduleName ?? type.Name;
      ModuleOption.ModuleMap.AddOrUpdate(name, type, (s, t) => t);
      if (isAssembly)
      {
        ModuleOption.ModuleAssemblyMap.AddOrUpdate(name, type, (s, t) => t);
      }
    }
    public static ServiceModule[] GetModuleBases(IServiceProvider sp)
    {
      return ModuleOption.ModuleMap
            .Select(b => sp.GetService(b.Value) as ServiceModule)
            .Where(b => b != null)
            .Select(b => b!)
            .OrderByDescending(b => b.Index).ToArray();
    }
  }
}
