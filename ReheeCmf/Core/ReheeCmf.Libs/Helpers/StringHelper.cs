﻿using ReheeCmf.Commons.Jsons.Options;
using System.Text.Json;

namespace ReheeCmf.Helpers
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
          return JsonSerializer.Deserialize<string[]>(input, JsonOption.DefaultOption)!;
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
        return JsonSerializer.Serialize(input, JsonOption.DefaultOption);
      }
      else
      {
        return String.Join(stringDelimiter, input!);
      }
    }
  }
}
