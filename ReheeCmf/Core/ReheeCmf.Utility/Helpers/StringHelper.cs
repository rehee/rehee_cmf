using System;
using System.Text.RegularExpressions;

namespace ReheeCmf.Helpers
{
  public static class StringHelper
  {
    public static string SplitPascalCase(this string input)
    {
      Regex r = new Regex(
        @"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])"
      );
      return r.Replace(input, " ");
    }

    public static Type GetSystemType(this TypeCode typeCode)
    {
      switch (typeCode)
      {
        case TypeCode.Boolean:
          return typeof(bool);
        case TypeCode.Char:
          return typeof(char);
        case TypeCode.SByte:
          return typeof(sbyte);
        case TypeCode.Byte:
          return typeof(byte);
        case TypeCode.Int16:
          return typeof(Int16);
        case TypeCode.UInt16:
          return typeof(UInt16);
        case TypeCode.Int32:
          return typeof(Int32);
        case TypeCode.UInt32:
          return typeof(UInt32);
        case TypeCode.Int64:
          return typeof(Int64);
        case TypeCode.UInt64:
          return typeof(UInt64);
        case TypeCode.Single:
          return typeof(Single);
        case TypeCode.Double:
          return typeof(Double);
        case TypeCode.Decimal:
          return typeof(decimal);
        case TypeCode.DateTime:
          return typeof(DateTime);
        case TypeCode.String:
          return typeof(string);
        case TypeCode.Object:
          return typeof(object);
      }
      return typeof(object);
    }
  }
}
