using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ODatas.Attributes
{
  public class ODataMappingAttribute : Attribute
  {
    public ODataMappingAttribute(Type type, bool ignore = false, object a = null)
    {
      Type = type;
      Ignore = ignore;
    }

    public Type Type { get; }
    public bool Ignore { get; }
  }
}
