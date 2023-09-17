using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ReheeCmf.Modules.ApiVersions;
using ReheeCmf.Modules;
using ReheeCmf.Helpers;
using System.IO;
using ReheeCmf.ContextModule;
using ReheeCmf.EntityModule;
using ReheeCmf.UserManagementModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ReheeCmf.Entities;
using ReheeCmf.ContextModule.Entities;
using ReheeCmf.FileModule;
using ReheeCmf.Modules.Permissions;
using ReheeCmf.AuthenticationModule;
using ReheeCmf.Commons.Jsons.Options;

namespace ReheeCmf
{
  public abstract class CmfApiModule<TContext, TUser> : ServiceModule
    where TContext : DbContext
    where TUser : IdentityUser, ICmfUser, new()
  {
    public override IEnumerable<ModuleDependOn> Depends()
    {
      return ModuleHelper.Depends(
        ModuleDependOn.New<CmfContextModule<TContext, TUser>>(),
        ModuleDependOn.New<CmfEntityModule>(),
        ModuleDependOn.New<CmfAuthenticationModule<TUser>>(),
        ModuleDependOn.New<CmfUserManagementModule<TUser, TenantIdentityRole, TenantIdentityUserRole>>(),
        ModuleDependOn.New<CmfFileModule>()
        );
    }

    public override void JsonConfiguration(ServiceConfigurationContext context)
    {
      base.JsonConfiguration(context);
      context.MvcBuilder!
        .AddJsonOptions(options =>
        {
          JsonOption.SetDefaultJsonSerializerOptions(options.JsonSerializerOptions);
        });
      //  .AddNewtonsoftJson(opts =>
      //{
      //  opts.SerializerSettings.ContractResolver = new DefaultContractResolver();
      //  opts.SerializerSettings.Converters.Add(new StringEnumConverter());
      //  opts.SerializerSettings.Converters.Add(new IsoDateTimeConverter2());
      //  opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
      //});

    }

    public override void SwaggerConfiguration(SwaggerGenOptions setupAction)
    {
      setupAction.SwaggerDoc("v0", new Microsoft.OpenApi.Models.OpenApiInfo()
      {
        Title = "api v0",
        Version = "v1",
      });
    }
    public override void SwaggerConfigurationWithApiVersion(
      SwaggerGenOptions setupAction, IApiVersionDescriptionProvider provider, ISwaggerApiVersion swaggerApiVersion)
    {
      if (provider != null)
      {
        foreach (var description in provider.ApiVersionDescriptions)
        {
          var info = swaggerApiVersion.GetSwaggerApiVersion(description);
          setupAction.SwaggerDoc(
              description.GroupName,
              info);
        }
      }
      else
      {
        var nullInfo = swaggerApiVersion.GetSwaggerApiVersion(null);
        setupAction.SwaggerDoc("v0", nullInfo);
      }
    }
  }
}
