using System.Reflection;


namespace ReheeCmf.Helper
{
  public static class ReflectTypeMapperHelper
  {
    public static TypeMap GetMap(this object input)
    {
      var type = input.GetType();
      if (type.IsProxy())
      {
        type = type.BaseType;
      }
      if (ReflectPool.TypeMapping.TryGetValue(type, out var map))
      {
        return map;
      }
      var result = new TypeMap(type);
      ReflectPool.TypeMapping.AddOrUpdate(type, result, (k, o) => result);
      return result;
    }

    public static TypeMap GetMap(this Type type)
    {
      var checkedType = type.ThisType();
      if (ReflectPool.TypeMapping.TryGetValue(checkedType, out var map))
      {
        return map;
      }
      var result = new TypeMap(checkedType);
      ReflectPool.TypeMapping.AddOrUpdate(checkedType, result, (k, o) => result);
      return result;
    }

    public static TypeMap GetMap(this string typeName)
    {
      var mapping = ReflectPool.TypeMapping.Values.FirstOrDefault(b => b.FullName == typeName) ??
        ReflectPool.TypeMapping.Values.FirstOrDefault(b => b.Name == typeName);
      if (mapping != null)
      {
        return mapping;
      }
      var allType = AllType();
      var type = allType.FirstOrDefault(b => b.FullName == typeName) ??
        allType.FirstOrDefault(b => b.Name == typeName);
      if (type == null)
      {
        return null;
      }
      return type.GetMap();
    }
    public static PropertyInfoMap GetMap(this PropertyInfo property)
    {
      if (ReflectPool.PropertyMapping.TryGetValue(property, out var map))
      {
        return map;
      }
      var result = new PropertyInfoMap(property);
      ReflectPool.PropertyMapping.AddOrUpdate(property, result, (k, o) => result);
      return result;
    }
    public static FieldInfoMap GetMap(this FieldInfo input)
    {

      if (ReflectPool.FieldMapping.TryGetValue(input, out var map))
      {
        return map;
      }
      var result = new FieldInfoMap(input);
      ReflectPool.FieldMapping.AddOrUpdate(input, result, (k, o) => result);
      return result;
    }
    public static MethodInfoMap GetMap(this MethodInfo input)
    {

      if (ReflectPool.MethodInfoMapping.TryGetValue(input, out var map))
      {
        return map;
      }
      var result = new MethodInfoMap(input);
      ReflectPool.MethodInfoMapping.AddOrUpdate(input, result, (k, o) => result);
      return result;
    }
    public static T GetAttribute<T>(this PropertyInfo property) where T : Attribute
    {
      var attr = property.GetMap().Attributes.FirstOrDefault(b => b.GetType() == typeof(T));
      if (attr == null)
      {
        return null;
      }
      return (T)attr;
    }


    public static Type[] AllType()
    {
      return AppDomain.CurrentDomain.GetAssemblies()
        .Select(b => b.GetTypes()).SelectMany(b => b).ToArray();
    }

  }
}
