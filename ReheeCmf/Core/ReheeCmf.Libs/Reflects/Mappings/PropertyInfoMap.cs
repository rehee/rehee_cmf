using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Reflects.Mappings
{
  public class PropertyInfoMap
  {
    public PropertyInfoMap(PropertyInfo property)
    {
      Attributes = property.GetCustomAttributes().ToArray();
      Name = property.Name;
      Property = property;
      HasElementType = property.PropertyType.HasElementType ||
        (property.PropertyType.IsImplement<System.Collections.IEnumerable>() && property.PropertyType.IsConstructedGenericType);
      var baseType = property.PropertyType.GetElementType() ?? property.PropertyType.GenericTypeArguments.FirstOrDefault();


      ElementType = HasElementType ? baseType : null;
      if (baseType != null)
      {
        BaseName = baseType.Name;
        BaseType = baseType;
      }

    }
    public string Name { get; }
    public PropertyInfo Property { get; }
    public Attribute[] Attributes { get; }
    public bool HasElementType { get; }
    public Type? ElementType { get; }
    public string? BaseName { get; set; }
    public Type? BaseType { get; set; }
  }
}
