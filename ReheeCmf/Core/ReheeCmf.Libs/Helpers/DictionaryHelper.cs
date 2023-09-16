using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Helpers
{
  public static class DictionaryHelper
  {
    public static bool TryGetValueStringKey<K>(this IDictionary<string, K?> dictionary, string key, out K? k)
    {
      var dKey = dictionary.Keys.FirstOrDefault(b => String.Equals(b, key, StringComparison.OrdinalIgnoreCase));
      if (dKey == null)
      {
        k = default(K?);
        return false;
      }
      return dictionary.TryGetValue(dKey, out k);
    }
    public static Dictionary<string, K?> ToDictionWithKeys<K>(this IDictionary<string, K?> dictionary, IEnumerable<string> keys)
    {
      var dKeys = dictionary.Keys.Where(b => keys.Any(k => string.Equals(k, b, StringComparison.OrdinalIgnoreCase)));

      return dictionary.Where(b => dKeys.Contains(b.Key)).ToDictionary();
    }
  }
}
