using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Modules
{
  public class ModuleDependOn
  {
    public static ModuleDependOn New<T>() where T : class
    {
      var type = typeof(T);
      return new ModuleDependOn()
      {
        DependType = typeof(T),
        GenericType = type.GenericTypeArguments.ToArray(),
      };
    }
    public Type? DependType { get; set; }
    public Type[]? GenericType { get; set; }
  }
}
