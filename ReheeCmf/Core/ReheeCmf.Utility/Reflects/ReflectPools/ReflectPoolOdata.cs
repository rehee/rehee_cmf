using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Reflects.ReflectPools
{
  public static partial class ReflectPool
  {
    public static object OdataEdmModelEntity { get; set; }
    public static ConcurrentDictionary<Type, string[]> EntityTypeQueryFirst { get; set; } = new ConcurrentDictionary<Type, string[]>();
    public static object OdataEdmModelQueryDTO { get; set; }
  }
}
