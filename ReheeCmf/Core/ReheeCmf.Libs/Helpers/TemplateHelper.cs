using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReheeCmf.Helpers
{
  public static class TemplateHelper
  {
    public static string Formate(this string templateString, Dictionary<string, string> mapping, RegexOptions option = RegexOptions.IgnoreCase)
    {
      var reg = new Regex(Common.TemplatepPttern, option);
      StringComparison compare = option == RegexOptions.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
      MatchEvaluator rep = m =>
      {
        if (!m.Success)
        {
          return m.Value;
        }
        var key = m.Value.Substring(1, m.Value.Length - 2);
        if (String.IsNullOrEmpty(key))
        {
          return m.Value;
        }
        return
          mapping.Where(b => b.Key.Equals(key, compare)).Select(b => b.Value).FirstOrDefault() ??
          m.Value;
      };
      return reg.Replace(templateString, rep);
    }
  }
}
