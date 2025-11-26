using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Reflects.ReflectPools
{
  public static partial class ReflectPool
  {
    public static MethodInfo ToArray => typeof(Enumerable).GetMethod(nameof(Enumerable.ToArray));
  }
}
