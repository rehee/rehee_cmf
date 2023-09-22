using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.Jsons.Converters
{
  public class UtcDateTimeConverter : JsonConverter<DateTime>
  {
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      if (DateTime.TryParseExact(reader.GetString(), Common.DateTimeFormats, Common.Culture, DateTimeStyles.AssumeUniversal, out DateTime result))
      {
        return result;
      }
      throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
      var utc = DateTime.SpecifyKind(value, DateTimeKind.Utc);
      writer.WriteStringValue(utc.ToString(Common.DATETIMEUTC, CultureInfo.InvariantCulture));
    }

  }
  public class UtcDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
  {
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      if (DateTime.TryParseExact(reader.GetString(), Common.DateTimeFormats, Common.Culture, DateTimeStyles.AssumeUniversal, out DateTime result))
      {
        return result;
      }
      throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
      writer.WriteStringValue(value.ToString(Common.DATETIMEUTC, CultureInfo.InvariantCulture));
    }

  }
}
