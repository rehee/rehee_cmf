using Microsoft.Extensions.Configuration;

namespace ReheeCmf.Helpers
{
  public static class ConfigrationHelper
  {
    public static TOption GetOption<TOption>(this IConfiguration configuration, string key = default, TOption defaultValue = default) where TOption : new()
    {
      var name = key ?? typeof(TOption).Name;
      var configValue = configuration.GetSection(name).Get<TOption>();
      return configValue ?? defaultValue ?? new TOption();
    }
  }

}