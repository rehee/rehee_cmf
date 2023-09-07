using ReheeCmf.Reflects.Mappings;
using System.Collections.Concurrent;
using System.Reflection;


namespace ReheeCmf.Reflects.ReflectPools
{
  public static partial class ReflectPool
  {
    public static ConcurrentDictionary<string, Assembly> AssemblyPool { get; set; } = new ConcurrentDictionary<string, Assembly>();
    public static ConcurrentDictionary<Type, TypeMap> TypeMapping { get; set; } = new ConcurrentDictionary<Type, TypeMap>();
    public static ConcurrentDictionary<PropertyInfo, PropertyInfoMap> PropertyMapping { get; set; } = new ConcurrentDictionary<PropertyInfo, PropertyInfoMap>();
    public static ConcurrentDictionary<FieldInfo, FieldInfoMap> FieldMapping { get; set; } = new ConcurrentDictionary<FieldInfo, FieldInfoMap>();
    public static ConcurrentDictionary<MethodInfo, MethodInfoMap> MethodInfoMapping { get; set; } = new ConcurrentDictionary<MethodInfo, MethodInfoMap>();
    public static ConcurrentDictionary<string, PermissionAttribute> NamePermissionMap { get; set; } = new ConcurrentDictionary<string, PermissionAttribute>();
  }
}
