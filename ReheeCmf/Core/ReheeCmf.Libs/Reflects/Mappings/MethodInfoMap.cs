using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Reflects.Mappings
{
  public class MethodInfoMap
  {
    public MethodInfoMap(MethodInfo methodInfo)
    {
      Attributes = methodInfo.GetCustomAttributes().ToArray();
      Name = methodInfo.Name;
      Method = methodInfo;
    }
    public string Name { get; }
    public MethodInfo Method { get; }
    public Object[] Attributes { get; }
  }
}
