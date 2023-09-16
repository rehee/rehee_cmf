using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Commons;
using ReheeCmf.FileModule.Services;
using ReheeCmf.FileServices;
using ReheeCmf.Helpers;
using ReheeCmf.Services;
using ReheeCmf.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.FileModule
{
  public static class CmfFileModuleSetup
  {
    public static IServiceCollection AddCmfFileService(
      this IServiceCollection services, IConfiguration configuration)
    {
      var path = Directory.GetCurrentDirectory();
      var option = configuration.GetOption<FileServiceOption>() ?? new FileServiceOption();
      option.ServerPath = path;

      services.AddSingleton<FileServiceOption>(sp => option);
      services.AddScoped<IFileService>(sp =>
      {
        return new CmfFileService(
          sp.GetService<FileServiceOption>()!,
          sp.GetRequiredService<IHttpClientFactory>(),
          sp.GetService<IContextScope<Tenant>>()!);
      });
      return services;
    }
  }
}
