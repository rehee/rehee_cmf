using ReheeCmf.Commons.Jsons.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReheeCmf.Commons.Jsons.Options
{
  public static class JsonOption
  {
    public static JsonSerializerOptions? _defaultOption { get; set; }
    public static JsonSerializerOptions DefaultOption
    {
      get
      {
        if (_defaultOption == null)
        {
          _defaultOption = new JsonSerializerOptions();
          SetDefaultJsonSerializerOptions(_defaultOption);
        }
        return _defaultOption;
      }
      set
      {
        _defaultOption = value;
      }
    }

    public static Action<JsonSerializerOptions?> SetDefaultJsonSerializerOptions { get; set; } = (JsonSerializerOptions? options) =>
    {
      if (options == null)
      {
        return;
      }
      options.PropertyNameCaseInsensitive = true;
      options.Converters.Add(new JsonStringEnumConverter());
      options.Converters.Add(new UtcDateTimeConverter());
      options.Converters.Add(new UtcDateTimeOffsetConverter());
    };

  }
}
