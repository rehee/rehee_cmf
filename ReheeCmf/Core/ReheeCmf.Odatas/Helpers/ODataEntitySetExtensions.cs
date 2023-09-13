using ReheeCmf.Attributes;
using ReheeCmf.Helpers;
using ReheeCmf.ODatas.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ODatas.Helpers
{
  public static class ODataEntitySetExtensions
  {
    public static void SetODataMapping(Type type, object odata)
    {
      SetOData(type, odata);
    }
    public static void SetODataProperty(Type type, object odata)
    {
      var staticFields = type.GetFields().Where(b => b.IsStatic);
      if (!staticFields.Any())
      {
        return;
      }
      var propertyName = "Property";
      var propertyMethods = odata.GetType().GetAllMethods()
        .Select(b =>
          (method: b, parameterName: b.GetParameters().Select(b => b.ParameterType.FullName).ToArray())
          )
        .Where(b => b.method.Name == propertyName)
        .Where(b2 => b2.parameterName.Length == 1)
        .ToArray();
      foreach (var field in staticFields)
      {
        if (field.HasCustomAttribute<IgnoreMappingAttribute>())
        {
          continue;
        }
        var fieldParameterName = field.FieldType.FullName;
        var m = propertyMethods.Where(b => b.parameterName.Contains(fieldParameterName)).Select(b => b.method).FirstOrDefault();
        if (m == null)
        {
          continue;
        }
        m.Invoke(odata, new object[] { field.GetValue(null) });
      }
    }
    public static void SetOData(Type type, object odata)
    {
      var staticFields = type.GetFields().Where(b => b.IsStatic);
      var staticMethods = type.GetAllMethods().Where(b => b.IsGenericMethod)
        .Select(b => (method: b, attr: b.GetCustomAttributes<ODataMappingAttribute>()))
        .Where(b => b.attr != null)
        .ToArray();

      var ignoreName = "Ignore";
      var propertyName = "Property";
      var ignoreMethod = odata.GetType().GetMethods().Where(b => b.Name.Equals(ignoreName) && b.IsGenericMethod).FirstOrDefault();
      var propertyGenericMethods = odata.GetType().GetMethods().Where(b => b.Name.Equals(propertyName) && b.IsGenericMethod).Select(b =>
      (method: b, generic: b.GetGenericArguments()));
      var nullableMethod = propertyGenericMethods.Where(b => b.generic.Any(b => b.IsNullable())).Select(b => b.method).FirstOrDefault();
      var normalMethod = propertyGenericMethods.Where(b => b.generic.All(b => !b.IsNullable())).Select(b => b.method).FirstOrDefault();
      var otype = odata.GetType();

      var allMethods = odata.GetType().GetAllMethods();

      var propertyMethods = allMethods
        .Select(b =>
          (method: b, parameterName: b.GetParameters().Select(b => b.ParameterType.FullName).ToArray(), parameters: b.GetParameters())
          )
        .Where(b => b.method.Name == propertyName)
        .Where(b2 => b2.parameterName.Length == 1)
        .ToArray();
      var genericMethod = allMethods
        .Select(b =>
          (
          method: b, parameterName: b.GetParameters().Select(b => b.ParameterType.FullName).ToArray(), parameters: b.GetParameters())
          )
        .Where(b => b.method.IsGenericMethod)
        .Where(b => b.method.Name.StartsWith(propertyName))
        .Where(b2 => b2.parameterName.Length == 1)

        .ToArray();

      foreach (var field in staticFields)
      {
        var attr = field.GetCustomAttributes<ODataMappingAttribute>();
        var fieldValue = field.GetValue(null);
        if (attr != null && attr.Ignore)
        {
          ignoreMethod.MakeGenericMethod(attr.Type).Invoke(odata, new object[] { fieldValue });
        }
        else
        {
          var fieldParameterName = field.FieldType.FullName;
          var m = propertyMethods.Where(b => b.parameterName.Contains(fieldParameterName)).Select(b => b.method).FirstOrDefault();
          if (m == null)
          {
            continue;
          }
          m.Invoke(odata, new object[] { fieldValue });
        }
      }
      foreach (var staticMethod in staticMethods)
      {
        var expressMethod = staticMethod.method.MakeGenericMethod(type).Invoke(null, null);
        if (staticMethod.attr.Ignore)
        {
          ignoreMethod.MakeGenericMethod(staticMethod.attr.Type).Invoke(odata, new object[] { expressMethod });
        }
        else
        {
          var ex = expressMethod.GetType();
          var fieldParameterName = fullNames(expressMethod.GetType());
          var f2 = fieldParameterName.
            SelectMany(b => b.Split(",").ToArray())
            .Where(b => b.StartsWith("[")).Distinct()
            .FirstOrDefault();
          var m = propertyMethods
            .Where(b => b.parameterName.Any(b => fieldParameterName.Contains(b)
            ))
            .Select(b => b.method).FirstOrDefault();
          if (m != null)
          {
            m.Invoke(odata, new object[] { expressMethod });
            continue;
          }

          var typeName = fieldParameterName.SelectMany(b => b.Split(",")).Where(b => b.StartsWith("[")).Distinct().ToArray();
          foreach (var g in genericMethod)
          {
            var gm = g.method.MakeGenericMethod(staticMethod.attr.Type);
            var gmp = gm.GetParameters().SelectMany(b => b.ParameterType.FullName.Split(",")).Where(b => b.StartsWith("[")).Distinct().ToArray();
            try
            {
              if (gmp.Any(b => typeName.Contains(b)))
              {
                gm.Invoke(odata, new object[] { expressMethod });
                break;
              }
            }
            catch
            {
              break;
            }
          }

          continue;
        }
      }
    }
    private static string[] fullNames(Type type, string[] names = null)
    {
      var nameList = names == null ? new string[] { type.FullName } : names.Concat(new string[] { type.FullName }).ToArray();
      if (type.BaseType != null)
      {
        return fullNames(type.BaseType, nameList);
      }
      return nameList;
    }
  }
}
