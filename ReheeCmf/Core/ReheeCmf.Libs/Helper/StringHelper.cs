using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReheeCmf.Helper
{
  public static class StringHelper
  {
    public static IEnumerable<string> ToIEnumerable(this string? input, string? stringDelimiter = null)
    {
      if (String.IsNullOrEmpty(input))
      {
        return Enumerable.Empty<string>();
      }
      try
      {
        if (!String.IsNullOrEmpty(stringDelimiter) && input.Contains(stringDelimiter))
        {
          return input.Split(stringDelimiter);
        }
        if (input.Length >= 2 && input.FirstOrDefault() == '[' && input.LastOrDefault() == ']')
        {
          return JsonSerializer.Deserialize<string[]>(input)!;
        }
        return new string[] { input };
      }
      catch (Exception ex)
      {
        throw ex.ThrowStatusException();
      }
    }
    public static string BackToString(this IEnumerable<string>? input, string? stringDelimiter = null)
    {
      if (input?.Any() == false)
      {
        return String.Empty;
      }
      if (String.IsNullOrEmpty(stringDelimiter))
      {
        if (input!.Count() == 1)
        {
          return input!.FirstOrDefault()!;
        }
        return JsonSerializer.Serialize(input);
      }
      else
      {
        return String.Join(stringDelimiter, input!);
      }
    }
  }
}
