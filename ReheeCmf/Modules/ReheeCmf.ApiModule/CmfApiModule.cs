using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ReheeCmf.Modules.ApiVersions;
using ReheeCmf.Modules;

namespace ReheeCmf
{
  public abstract class CmfApiModule : ServiceModule
  {
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
