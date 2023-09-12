using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Helpers
{
  public static class AttributeHelper
  {
    public static T? GetTypedAttribute<T>(this Type entityType) where T : Attribute
    {
      foreach (var a in entityType.GetCustomAttributes(true))
      {
        if (a is T eh)
        {
          return eh;
        }
      }
      return null;
    }

    public static T? GetCustomAttributes<T>(this PropertyInfo property) where T : Attribute
    {
      return property.GetCustomAttribute<T>();
    }

    public static bool HasCustomAttribute<T>(this PropertyInfo property) where T : Attribute
    {
      return property.HasCustomAttribute(typeof(T));
    }
    public static bool HasCustomAttribute(this PropertyInfo property, Type type)
    {
      return property.GetMap().Attributes.Any(b => b.GetType() == type);
    }
    public static bool HasCustomAttribute(this PropertyInfo property, string name)
    {
      return property.GetMap().Attributes.Any(b =>
      {
        var map = b.GetType().GetMap();
        return map.FullName == name || map.Name == name;
      });
    }

    public static bool HasCustomAttribute<T>(this Type type) where T : Attribute
    {
      return type.HasCustomAttribute(typeof(T));
    }
    public static bool HasCustomAttribute<T>(this FieldInfo type) where T : Attribute
    {
      var t = typeof(T);
      return type.GetCustomAttributes(t).Any() == true;
    }
    public static bool HasCustomAttribute(this Type property, Type type)
    {
      return property.GetMap().Attributes.Any(b => b.GetType() == type);
    }
    public static bool HasCustomAttribute(this Type property, string name)
    {
      return property.GetMap().Attributes.Any(b =>
      {
        var map = b.GetType().GetMap();
        return map.FullName == name || map.Name == name;
      });
    }

    public static T? GetCustomAttributes<T>(this Type property)
    {
      var mapping = property.GetMap();
      foreach (var attr in mapping.Attributes)
      {
        if (attr is T tA)
        {
          return tA;
        }
      }
      return default(T?);
    }
    public static T? GetCustomAttributes<T>(this FieldInfo fields)
    {
      var mapping = fields.GetMap();
      foreach (var attr in mapping.Attributes)
      {
        if (attr is T tA)
        {
          return tA;
        }
      }
      return default(T?);
    }
    public static T? GetCustomAttributes<T>(this MethodInfo method)
    {
      var mapping = method.GetMap();
      foreach (var attr in mapping.Attributes)
      {
        if (attr is T tA)
        {
          return tA;
        }
      }
      return default(T?);
    }
  }
}
