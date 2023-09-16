
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ReheeCmf.Helpers
{
  public static class ConfigurationHelper
  {
    public static TOption GetOption<TOption>(this IConfiguration configuration, string? key = default, TOption? defaultValue = default) where TOption : new()
    {
      var name = key ?? typeof(TOption).Name;
      var configValue = configuration.GetSection(name).Get<TOption>();
      return configValue ?? defaultValue ?? new TOption();
    }
  }
  public static class SetupFunction
  {
    public static IServiceCollection AddingHttpClient(
      this IServiceCollection services, string httpClientName)
    {
      services.AddHttpClient(httpClientName, (sp, client) =>
      {
        var context = sp.GetServices<IHttpContextAccessor>();
        var current = context.FirstOrDefault().HttpContext;
        var options = sp.GetService<CrudOption>();
        var baseUrl = "http://localhost:8888";
        try
        {
          baseUrl = options.EntityQueryUri ?? $"{current.Request.Scheme}://{current.Request.Host}{current.Request.PathBase}";

        }
        catch
        {

        }
        client.BaseAddress = new Uri(baseUrl);
      });
      return services;
    }


  }
}