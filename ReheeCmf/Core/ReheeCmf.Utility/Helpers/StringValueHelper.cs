
using ReheeCmf.Commons.Jsons.Options;
using System.Globalization;
using System.Text.Json;

namespace ReheeCmf.Helpers
{
	public static class StringValueHelper
	{
		public static ContentResponse<T> GetObjValue<T>(this string? input)
		{
			var result = new ContentResponse<T>();
			var response = GetObjValue(input, typeof(T));
			if (response.Success && response.Content is T tValue)
			{
				result.SetSuccess(tValue);
			}
			return result;
		}
		public static ContentResponse<object> GetObjValue(this string? input, Type type, TypeCode? typeCode = null, Type? originalType = null, bool isNullable = false)
		{
			if (typeCode == null)
			{
				var nullableCheck = type.IsNullable();
				if (nullableCheck)
				{
					var mapper = type.GetMap();
					var genericType = mapper.GenericTypeArguments.FirstOrDefault();
					return GetObjValue(input, genericType!, Type.GetTypeCode(genericType), type, nullableCheck);
				}
				return GetObjValue(input, type, Type.GetTypeCode(type), type, nullableCheck);
			}
			var result = new ContentResponse<object>();
			Func<ContentResponse<object>> returnIfNullable = () =>
			{
				if (isNullable)
				{
					result.SetSuccess(null);
					return result;
				}
				return result;
			};
			switch (typeCode)
			{
				case TypeCode.Boolean:
					if (bool.TryParse(input, out var boolValue))
					{
						result.SetSuccess(boolValue);
						return result;
					}
					return returnIfNullable();
				case TypeCode.Char:
					if (char.TryParse(input, out var charValue))
					{
						result.SetSuccess(charValue);
						return result;
					}
					return returnIfNullable();
				case TypeCode.SByte:
					if (SByte.TryParse(input, out var sByteValue))
					{
						result.SetSuccess(sByteValue);
						return result;
					}
					return returnIfNullable();
				case TypeCode.Byte:
					if (Byte.TryParse(input, out var byteValue))
					{
						result.SetSuccess(byteValue);
						return result;
					}
					return returnIfNullable();
				case TypeCode.Int16:
					if (Int16.TryParse(input, out var Int16Value))
					{
						result.SetSuccess(Int16Value);
						return result;
					}
					return returnIfNullable();
				case TypeCode.UInt16:
					if (UInt16.TryParse(input, out var UInt16Value))
					{
						result.SetSuccess(UInt16Value);
						return result;
					}
					return returnIfNullable();
				case TypeCode.Int32:
					if (originalType!.IsEnum)
					{
						if (Enum.TryParse(originalType, input, out var enumResult))
						{
							result.SetSuccess(enumResult);
							return result;
						}
						return returnIfNullable();
					}
					if (type!.IsEnum)
					{
						if (Enum.TryParse(type, input, out var enumResult))
						{
							result.SetSuccess(enumResult);
							return result;
						}
						return returnIfNullable();
					}
					if (Int32.TryParse(input, out var Int32Value))
					{
						result.SetSuccess(Int32Value);
						return result;
					}
					return returnIfNullable();
				case TypeCode.UInt32:
					if (UInt32.TryParse(input, out var UInt32Value))
					{
						result.SetSuccess(UInt32Value);
						return result;
					}
					return returnIfNullable();
				case TypeCode.Int64:
					if (Int64.TryParse(input, out var Int64Value))
					{
						result.SetSuccess(Int64Value);
						return result;
					}
					return returnIfNullable();
				case TypeCode.UInt64:
					if (UInt64.TryParse(input, out var UInt64Value))
					{
						result.SetSuccess(UInt64Value);
						return result;
					}
					return returnIfNullable();
				case TypeCode.Single:
					if (Single.TryParse(input, out var SingleValue))
					{
						result.SetSuccess(SingleValue);
						return result;
					}
					return returnIfNullable();
				case TypeCode.Double:
					if (Double.TryParse(input, out var DoubleValue))
					{
						result.SetSuccess(DoubleValue);
						return result;
					}
					return returnIfNullable();
				case TypeCode.Decimal:
					if (Decimal.TryParse(input, out var DecimalValue))
					{
						result.SetSuccess(DecimalValue);
						return result;
					}
					return returnIfNullable();
				case TypeCode.String:
					result.SetSuccess(input);
					return result;
				case TypeCode.DateTime:

					if (DateTime.TryParseExact(input, Common.DateTimeFormats, Common.DateCulture, DateTimeStyles.AssumeUniversal, out var date))
					{
						result.SetSuccess(date);
						return result;
					}

					return returnIfNullable();
				case TypeCode.Object:
					switch (type.Name)
					{
						case nameof(DateTimeOffset):
							if (DateTimeOffset.TryParseExact(input, Common.DateTimeFormats, Common.DateCulture, DateTimeStyles.AssumeUniversal, out var dateOffSet))
							{
								result.SetSuccess(dateOffSet);
								return result;
							}
							return returnIfNullable();
						case nameof(Guid):
							if (Guid.TryParse(input, out var guidValue))
							{
								result.SetSuccess(guidValue);
								return result;
							}
							return returnIfNullable();
						default:
							try
							{
								if (isNullable && String.IsNullOrEmpty(input))
								{
									return returnIfNullable();
								}
								var objFromJson = JsonSerializer.Deserialize(input!, originalType!, JsonOption.DefaultOption);
								result.SetSuccess(objFromJson);
								return result;
							}
							catch
							{
								return returnIfNullable();
							}
					}

			}
			return result;
		}
		public static ContentResponse<string> GetStrValue(this object? input, Type type, TypeCode? typeCode = null, Type? originalType = null, bool isNullable = false)
		{
			if (typeCode == null)
			{
				var nullableCheck = type.IsNullable();
				if (nullableCheck)
				{
					var mapper = type.GetMap();
					var genericType = mapper.GenericTypeArguments.FirstOrDefault();
					return GetStrValue(input, genericType!, Type.GetTypeCode(genericType), type, nullableCheck);
				}
				return GetStrValue(input, type, Type.GetTypeCode(type), type, nullableCheck);
			}
			var result = new ContentResponse<string>();
			Func<ContentResponse<string>> returnIfNullable = () =>
			{
				if (isNullable)
				{
					result.SetSuccess(null);
					return result;
				}
				return result;
			};
			if (input == null)
			{
				return returnIfNullable();
			}
			switch (typeCode)
			{
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
					result.SetSuccess(input.ToString());
					return result;
				case TypeCode.Decimal:
					if (input is Decimal DecimalValue)
					{
						result.SetSuccess(DecimalValue.ToString(Common.DigitalFormat));
					}
					return result;
				case TypeCode.Int32:
					if (originalType!.IsEnum)
					{
						result.SetSuccess(Enum.GetName(type, input));
						return result;
					}
					result.SetSuccess(input.ToString());
					return result;
				case TypeCode.String:
					result.SetSuccess(input);
					return result;
				case TypeCode.DateTime:
					if (input is DateTime DateTimeValue)
					{
						result.SetSuccess(DateTimeValue.ToString(Common.DATETIMEUTC));
						return result;
					}
					return returnIfNullable();
				case TypeCode.Object:
					switch (type.Name)
					{
						case nameof(DateTimeOffset):
							if (input is DateTimeOffset DateTimeOffsetValue)
							{
								result.SetSuccess(DateTimeOffsetValue.ToString(Common.DATETIMEUTC));
								return result;
							}
							return returnIfNullable();
						case nameof(Guid):
							if (input is Guid GuidValue)
							{
								result.SetSuccess(GuidValue.ToString());
								return result;
							}
							return returnIfNullable();
						default:
							try
							{
								result.SetSuccess(JsonSerializer.Serialize(input, JsonOption.DefaultOption));
								return result;
							}
							catch
							{
								return returnIfNullable();
							}
					}

			}
			return result;
		}

		public static string? StringValue(this object? input, Type? type = null)
		{
			if (input == null)
			{
				return null;
			}
			var response = GetStrValue(input, type ?? input.GetType());
			if (response.Success)
			{
				return response.Content;
			}
			return null;
		}
		public static T? GetValue<T>(this string? input)
		{
			var response = GetObjValue<T>(input);
			if (response.Success)
			{
				return response.Content;
			}
			return default(T?);
		}


	}

}
