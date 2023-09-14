using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Helpers
{
  public static class ServiceCollectionHelpers
  {
    public static IServiceCollection AddScopedWithDefault<TService, TDefault>(this IServiceCollection services, Func<IServiceProvider, TService> func)
       where TService : class
      where TDefault : class, TService
    {
      if (func != null)
      {
        services.AddScoped<TService>(func);
      }
      else
      {
        services.AddScoped<TService, TDefault>();
      }

      return services;
    }
    public static IServiceCollection AddTransientWithDefault<TService, TDefault>(this IServiceCollection services, Func<IServiceProvider, TService> func)
       where TService : class
      where TDefault : class, TService
    {
      if (func != null)
      {
        services.AddTransient<TService>(func);
      }
      else
      {
        services.AddTransient<TService, TDefault>();
      }

      return services;
    }
    public static IServiceCollection AddSingletonWithDefault<TService, TDefault>(this IServiceCollection services, Func<IServiceProvider, TService> func)
       where TService : class
      where TDefault : class, TService
    {
      if (func != null)
      {
        services.AddSingleton<TService>(func);
      }
      else
      {
        services.AddSingleton<TService, TDefault>();
      }

      return services;
    }


  }
}
