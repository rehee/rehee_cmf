using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Helpers
{
  public static class ApiVersionExtensions
  {
    public static void ConfigureApiVersioning(this IServiceCollection services, IConfiguration configuration, string apiTitle)
    {
      var swaggerApi = new SwaggerApiVersion(apiTitle);
      services.AddSingleton<ISwaggerApiVersion, SwaggerApiVersion>(
        sp => swaggerApi);
      var defaultVersion = configuration.GetSection(ApiVersionExtensions.CurrentApiVersionKey).Get<string>();
      services.AddApiVersioning(options =>
      {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = ApiVersion.Parse(defaultVersion ?? "1.0");
        options.ApiVersionReader = ApiVersionReader.Combine(
                 new HeaderApiVersionReader(VersionParamName));
        options.Conventions.Add(new VersionByNamespaceConvention());
      });

      services.AddVersionedApiExplorer(setup =>
      {
        setup.GroupNameFormat = ApiVersionGroupNameFormat;
        setup.SubstituteApiVersionInUrl = true;
      });
    }

    public static void CmfUseSwagger(this IApplicationBuilder app, IServiceProvider service)
    {
      var apis = service.GetService<IApiVersionDescriptionProvider>();
      var swagApi = service.GetService<ISwaggerApiVersion>();
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        if (apis?.ApiVersionDescriptions?.Any() == true)
        {
          foreach (var api in apis.ApiVersionDescriptions)
          {
            var detail = swagApi.GetSwaggerApiVersion(api);
            c.SwaggerEndpoint($"/swagger/{api.GroupName}/swagger.json", detail.Title);
          }
        }
        else
        {
          c.SwaggerEndpoint($"/swagger/v0/swagger.json", "Api");
        }
      });
    }



    public static void GetSwaggerApi(this ApiVersionDescription apiVersionDescription)
    {
      var apiGroup = apiVersionDescription.GroupName;

    }

    public const string ApiVersionGroupNameFormat = "'v'V.VV";
    public const string VersionParamName = "api-version";
    public const string CurrentApiVersionKey = "CurrentApiVersion";
  }
}
