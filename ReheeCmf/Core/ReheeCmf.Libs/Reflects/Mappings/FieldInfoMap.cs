using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Reflects.Mappings
{
  public class FieldInfoMap
  {
    public FieldInfoMap(FieldInfo fieldInfo)
    {
      Attributes = fieldInfo.GetCustomAttributes().ToArray();
      Name = fieldInfo.Name;
      Field = fieldInfo;
      HasElementType = fieldInfo.FieldType.HasElementType ||
        (fieldInfo.FieldType.IsImplement<System.Collections.IEnumerable>() && fieldInfo.FieldType.IsConstructedGenericType);
      var baseType = fieldInfo.FieldType.GetElementType() ?? fieldInfo.FieldType.GenericTypeArguments.FirstOrDefault();


      ElementType = HasElementType ? baseType : null;
      if (baseType != null)
      {
        BaseName = baseType.Name;
        BaseType = baseType;
      }

    }
    public string Name { get; }
    public FieldInfo Field { get; }
    public Object[] Attributes { get; }
    public bool HasElementType { get; }
    public Type? ElementType { get; }
    public string? BaseName { get; set; }
    public Type? BaseType { get; set; }
  }
}
