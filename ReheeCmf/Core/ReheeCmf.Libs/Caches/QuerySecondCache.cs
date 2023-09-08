using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Caches
{
  public class QuerySecondCache
  {
    public QuerySecondCache(bool enableCache = false)
    {
      EnableCache = enableCache;
    }
    public bool EnableCache { get; set; }
  }
}
