using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Reflects.ReflectPools
{
  public static partial class ReflectPool
  {
    public static ConcurrentDictionary<string, Type> EntityNameMapping { get; set; } = new ConcurrentDictionary<string, Type>();
    public static ConcurrentDictionary<Type, PropertyInfo> EntityKeyMapping { get; set; } = new ConcurrentDictionary<Type, PropertyInfo>();
    public static ConcurrentDictionary<Type, string> EntityMapping_2 { get; set; } = new ConcurrentDictionary<Type, string>();

    //public static MethodInfo IContextAdd { get; set; }
    //public static MethodInfo IContextAddT { get; set; }
    //public static MethodInfo IContextAddRange { get; set; }

    public static MethodInfo AsNoTracking { get; set; }
   
  }
  
}
