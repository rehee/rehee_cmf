using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Modules.ApiVersions
{
  public class SwaggerApiVersion : ISwaggerApiVersion
  {
    private readonly string apiTitle = string.Empty;

    public SwaggerApiVersion(string apiTitle)
    {
      this.apiTitle = apiTitle;
    }

    public OpenApiInfo GetSwaggerApiVersion(ApiVersionDescription apiVersionDescription)
    {
      if (apiVersionDescription != null)
      {
        var versionInUrl = apiVersionDescription.GroupName;
        var versionTitle = $"{apiTitle} {apiVersionDescription.ApiVersion.ToString()}";
        return new OpenApiInfo()
        {
          Title = versionTitle,
          Version = apiVersionDescription.ApiVersion.ToString(),
          Description = apiVersionDescription.IsDeprecated ? "This API version has been deprecated." : null,
        };
      }
      else
      {
        return new Microsoft.OpenApi.Models.OpenApiInfo()
        {
          Title = apiTitle,
          Version = "v0",
        };
      }




    }
  }
}
