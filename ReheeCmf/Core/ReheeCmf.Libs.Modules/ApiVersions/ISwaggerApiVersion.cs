using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace ReheeCmf.Libs.Modules.ApiVersions
{
  public interface ISwaggerApiVersion
  {
    OpenApiInfo GetSwaggerApiVersion(ApiVersionDescription apiVersionDescription);
  }
}
