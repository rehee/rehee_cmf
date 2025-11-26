using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReheeCmf.Helpers
{
  public static class TypeHelper
  {
    public static bool IsIEnumerable(this Type type, bool noString = true)
    {
      if (noString && Type.GetTypeCode(type) == TypeCode.String)
      {
        return false;
      }
      return type.GetInterfaces()
        .Any(t => t.Name == "IEnumerable" || t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }

    public static bool IsIEnumerable(this PropertyInfo property, bool noString = true)
    {
      return property.PropertyType.IsIEnumerable(noString);
    }

    public static bool IsNullable(this Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    public static bool IsNullable(this PropertyInfo property)
    {
      return property.PropertyType.IsNullable();
    }

    public static bool IsImplement(this Type type, Type targetType)
    {
      if (type == targetType)
      {
        return true;
      }
      if (targetType.IsInterface)
      {
        if (type.GetInterfaces().Any(b => b == targetType))
        {
          return true;
        }
      }
      else
      {
        var basetype = type.BaseType;
        while (basetype != null)
        {
          if (basetype == targetType)
          {
            return true;
          }
          basetype = basetype.BaseType;
        }
      }
      return false;
    }

    public static bool IsImplement<T>(this Type type)
    {
      return type.IsImplement(typeof(T));
    }

    public static bool IsSimpleType(this Type type)
    {
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.Object:
        case TypeCode.Empty:
          return false;
      }
      return true;
    }

    public static FieldInfo[] GetAllFields(this Type type, bool inherit = true, FieldInfo[]? lists = null)
    {
      if (!inherit)
      {
        return type.GetFields();
      }
      var fields = lists == null ? type.GetFields() : lists.Concat(type.GetFields()).ToArray();
      if (type.BaseType != null && type.BaseType != typeof(object))
      {
        return type.BaseType.GetAllFields(inherit, fields);
      }
      return fields;
    }

    public static MethodInfo[] GetAllMethods(this Type type, bool inherit = true, MethodInfo[]? lists = null)
    {
      if (!inherit)
      {
        return type.GetMethods();
      }
      var fields = lists == null ? type.GetMethods() : lists.Concat(type.GetMethods()).ToArray();
      if (type.BaseType != null && type.BaseType != typeof(object))
      {
        return type.BaseType.GetAllMethods(inherit, fields);
      }
      return fields;
    }

    public static bool IsInheritance(this Type type, Type check)
    {
      if (Type.Equals(type, check))
      {
        return true;
      }
      if (type.BaseType != null)
      {
        return type.BaseType.IsInheritance(check);
      }
      return false;
    }
  }
}
