using ReheeCmf.Responses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReheeCmf.Helper
{
  public static class StringValueHelper
  {
    public static string? StringValue(this object? input, Type? originalType = null)
    {
      if (input == null)
      {
        return null;
      }
      var result = input.GetStringValue(originalType);
      return result.Content;
    }
    public static bool TryStringValue(this object input, Type? originalType, out string? value)
    {
      var result = input.GetStringValue(originalType);
      value = result.Content;
      return result.Success;
    }
    public static ContentResponse<string?> GetStringValue(this object input, Type? originalType = null)
    {
      var result = new ContentResponse<string?>();
      if (input == null)
      {
        if (originalType != null && originalType.IsNullable())
        {
          result.SetSuccess(null);
        }
        else
        {
          result.Status = HttpStatusCode.NotFound;
        }

        return result;
      }
      var objectType = input.GetType();
      if (objectType.IsIEnumerable())
      {
        var lists = ((IEnumerable)input).Cast<object>().ToList();
        var listConvert = lists.Select(b => b.GetStringValue()).ToList();
        var stringValues = listConvert.Where(b => b.Success).Select(b => b.Content).ToList();
        var d = Common.StringDelimiter;
        var listResult = stringValues.BackToString();
        result.SetSuccess(listResult);
        return result;
      }
      var typeCode = Type.GetTypeCode(objectType);
      if (objectType.IsEnum)
      {
        return ((int)input).GetStringValue();
      }
      string format = null;
      string format2 = null;
      switch (typeCode)
      {
        case TypeCode.Boolean:
        case TypeCode.Char:
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
          goto invoke;
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          format = Common.DigitalFormat;
          goto invoke;
        case TypeCode.DateTime:
          format = Common.DataFormat;
          format2 = Common.DataFormat2;
          goto invoke;
        case TypeCode.String:
          result.SetSuccess<string>(input as string);
          return result;
      }
      switch (objectType.Name)
      {
        case nameof(DateTimeOffset):
          if (input != null)
          {
            var offset = (DateTimeOffset)input;
            return offset.DateTime.GetStringValue();
          }
          return result;
        case nameof(Guid):
          result.SetSuccess(input.ToString());
          return result;
      }
      result.SetError(HttpStatusCode.NotImplemented);
      return result;

    invoke:
      var dateConvert = objectType.Invoke<string>("ToString", input, format);
      if (dateConvert.Success)
      {
        result.SetSuccess(dateConvert.Content);
      }
      else
      {
        result.SetError(dateConvert);
      }
      return result;

      //boolConvert:
      //  switch(boolConvert)
      //  return result;

    }
    public static ContentResponse<string> GetStringValue(this PropertyInfo property, object item, Type originalType = null)
    {
      return property.GetValue(item).GetStringValue(originalType);
    }

    public static ContentResponse<T> GetValue<T>(this string input)
    {
      var result = new ContentResponse<T>();
      if (String.IsNullOrEmpty(input))
      {
        result.SetError(HttpStatusCode.NotFound);
        return result;
      }
      var objectType = typeof(T);
      if (objectType.IsIEnumerable())
      {
        var listType = typeof(List<>);
        var elementType = objectType.GetElementType() ?? objectType.GenericTypeArguments.FirstOrDefault();
        var constructedListType = listType.MakeGenericType(elementType);
        var instance = (IList)Activator.CreateInstance(constructedListType);

        var inputList = input.ToIEnumerable()
          .Select(b => b.GetValue(elementType))
          .Where(b => b.Success)
          .Select(b => b.Content);
        foreach (var i in inputList)
        {
          instance.Add(i);
        }
        if (objectType.IsArray)
        {
          var arrayResult = instance.GetType().Invoke<object>("ToArray", instance, null);
          if (arrayResult.Success)
          {
            result.SetSuccess((T)arrayResult.Content);
          }
          return result;
        }
        else
        {
          result.SetSuccess((T)instance);
        }
        return result;
      }
      var convertResult = input.GetValue(
        Type.GetTypeCode(objectType), objectType);
      if (!convertResult.Success)
      {
        result.SetError(convertResult);
      }
      else
      {
        result.SetSuccess((T)convertResult.Content);
      }
      return result;
    }
    public static ContentResponse<object> GetValue(this string input, Type type)
    {
      return input.GetValue(Type.GetTypeCode(type), type);
    }
    public static ContentResponse<object> GetValue(this string input, TypeCode typeCode, Type origilalType = null, bool isNullable = false)
    {
      var result = new ContentResponse<object>();
      if (string.IsNullOrEmpty(input))
      {
        if (isNullable)
        {
          result.SetError(HttpStatusCode.NotFound);
        }
        else
        {
          result.SetSuccess(null);
        }
        return result;
      }

      string format = null;
      string invokeMethod = "TryParse";
      ContentResponse<bool> convertResult;
      var type = typeCode.GetSystemType();
      object[] parameters;
      switch (typeCode)
      {
        case TypeCode.Boolean:
        //goto invokeBoolean;
        case TypeCode.Char:
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          goto invoke;
        case TypeCode.DateTime:
          format = Common.DataFormat;
          invokeMethod = "TryParseExact";
          goto invokeDate;
        case TypeCode.String:
          result.SetSuccess<object>(input);
          return result;
        case TypeCode.Object:
          if (origilalType.IsIEnumerable())
          {
            var baseEnumType = origilalType.GetElementType() ?? origilalType.GenericTypeArguments.FirstOrDefault();
            var d = Common.StringDelimiter;
            var values = input.ToIEnumerable()
              .Select(b => GetValue(b, baseEnumType))
              .Where(b => b.Success)
              .Select(b => b.Content).ToArray();
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(baseEnumType);
            var instance = (IList)Activator.CreateInstance(constructedListType);
            foreach (var v in values)
            {
              instance.Add(v);
            }
            if (origilalType.IsArray)
            {
              var arrayResult = instance.GetType().Invoke<object>("ToArray", instance, null);
              if (arrayResult.Success)
              {
                result.SetSuccess(arrayResult.Content);
              }
            }
            else
            {
              result.SetSuccess(instance as object);
            }
            return result;
          }

          return GetObjectValue(input, origilalType);
        default:
          result.SetError(HttpStatusCode.NotImplemented);
          return result;
      }

    invoke:
      if (typeCode == TypeCode.Int32 && origilalType != null && origilalType.IsEnum)
      {
        result = new ContentResponse<object>();
        if (Enum.TryParse(origilalType, input, out var enumResult))
        {
          result.SetSuccess(enumResult);
        }
        return result;
      }
      parameters = new object[] { input, null };
      convertResult = type.Invoke<bool>(invokeMethod, null, parameters);
      goto returnResult;

    invokeDate:
      //var dataFormat = String.Equals(input[input.Length - 1].ToString(), "z", StringComparison.InvariantCultureIgnoreCase);
      //parameters = new object[] { input, dataFormat ? format : Common.DataFormat2, null, DateTimeStyles.None, null };
      //convertResult = type.Invoke<bool>(invokeMethod, null, parameters);
      try
      {
        var inputData = $@"""{input}""";
        var date = JsonSerializer.Deserialize<DateTime>(inputData);
        result.SetSuccess(date);
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        result.SetError(ex);
      }
      return result;

    //invokeBoolean:

    //  if (String.IsNullOrEmpty(input))
    //  {
    //    result.SetError();
    //  }
    //  result.SetSuccess(string.Equals(input, "true", StringComparison.InvariantCultureIgnoreCase));
    //  return result;

    returnResult:

      if (convertResult.Success && convertResult.Content)
      {
        result.SetSuccess(parameters.LastOrDefault());
      }
      return result;
    }

    public static ContentResponse<object> GetObjectValue(string input, Type type, bool? isNullable = null)
    {
      var result = new ContentResponse<object>();
      if (type == null)
      {
        return result;
      }
      if (isNullable == null)
      {
        if (type.IsNullable())
        {
          var genericType = type.GenericTypeArguments.FirstOrDefault();
          return GetObjectValue(input, genericType, true);
        }
        else
        {
          return GetObjectValue(input, type, false);
        }
      }
      var baseType = Type.GetTypeCode(type);
      if (baseType == TypeCode.Object)
      {
        switch (type.Name)
        {
          case nameof(DateTimeOffset):
            var datatime = input.GetValue(TypeCode.DateTime);
            if (datatime.Success)
            {
              var v = (DateTime)datatime.Content;
              DateTimeOffset utcTime2 = DateTime.SpecifyKind(v, DateTimeKind.Utc);
              result.SetSuccess(utcTime2);
              return result;
            }
            break;
          case nameof(Guid):
            if (Guid.TryParse(input, out var guidValue))
            {
              result.SetSuccess(guidValue);
              return result;
            }
            break;
        }
      }
      else if (type.IsEnum && Enum.TryParse(type, input, out var enumResult))
      {
        result.SetSuccess(enumResult);
        return result;
      }
      else
      {
        return GetValue(input, baseType, type, isNullable == true);
      }

      if (isNullable == true)
      {
        result.SetSuccess(null);
      }
      return result;
    }

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
      return null;
    }

  }
}
