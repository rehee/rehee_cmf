using System.Collections.Generic;

namespace ReheeCmf.Helpers
{
  public static class CommonHelper
  {
    public static KeyValuePair<string, object> CreateStringObj(string key, object value)
    {
      return new KeyValuePair<string, object>(key, value);
    }
  }

  public static class KeyValuePairFunc
  {
    public static KeyValuePair<string, object> CreateStringObj(string key, object value)
    {
      return new KeyValuePair<string, object>(key, value);
    }
  }
}
