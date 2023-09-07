using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace ReheeCmf.Modules.ApiVersions
{
  public interface ISwaggerApiVersion
  {
    OpenApiInfo GetSwaggerApiVersion(ApiVersionDescription apiVersionDescription);
  }
}
