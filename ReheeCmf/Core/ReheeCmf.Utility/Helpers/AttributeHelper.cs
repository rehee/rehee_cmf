using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
      return property.GetCustomAttributes().Any(b => b.GetType() == type);
    }

    public static bool HasCustomAttribute(this PropertyInfo property, string name)
    {
      return property.GetCustomAttributes().Any(b =>
      {
        var bType = b.GetType();
        return bType.FullName == name || bType.Name == name;
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
      return property.GetCustomAttributes().Any(b => b.GetType() == type);
    }

    public static bool HasCustomAttribute(this Type property, string name)
    {
      return property.GetCustomAttributes().Any(b =>
      {
        var bType = b.GetType();
        return bType.FullName == name || bType.Name == name;
      });
    }

    public static T? GetCustomAttributes<T>(this Type property)
    {
      foreach (var attr in property.GetCustomAttributes())
      {
        if (attr is T tA)
        {
          return tA;
        }
      }
      return default(T);
    }

    public static T? GetCustomAttributes<T>(this FieldInfo fields)
    {
      foreach (var attr in fields.GetCustomAttributes())
      {
        if (attr is T tA)
        {
          return tA;
        }
      }
      return default(T);
    }

    public static T? GetCustomAttributes<T>(this MethodInfo method)
    {
      foreach (var attr in method.GetCustomAttributes())
      {
        if (attr is T tA)
        {
          return tA;
        }
      }
      return default(T);
    }
  }
}
