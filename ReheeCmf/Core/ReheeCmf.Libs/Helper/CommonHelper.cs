using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Helper
{
  public static class KeyValuePairFunc
  {
    public static KeyValuePair<string, object> CreateStringObj(string key, object value)
    {
      return new KeyValuePair<string, object>(key, value);
    }
  }
}
