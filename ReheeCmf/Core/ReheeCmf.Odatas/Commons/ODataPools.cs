using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ODatas.Commons
{
  public static partial class ODataPools
  {
    public static ConcurrentDictionary<string, Type> QueryNameTypeMapping = new ConcurrentDictionary<string, Type>();
    public static ConcurrentDictionary<string, Type> QueryNameKeyTypeMapping = new ConcurrentDictionary<string, Type>();
    public static ConcurrentDictionary<string, object> QueryNameBuilderMapping = new ConcurrentDictionary<string, object>();
  }
}
