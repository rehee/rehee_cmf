using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ODatas.Commons
{
  public class ODataEndpointMapping
  {
    public static ODataEndpointMapping New(string controller, string action, string path, string entityKey, string entityName = null)
    {
      return new ODataEndpointMapping()
      {
        Controller = controller,
        Action = action,
        Path = path,
        EntityKey = entityKey,
        EntityName = entityName
      };
    }
    public string? Controller { get; set; }
    public string? Action { get; set; }
    public string? Path { get; set; }
    public string? EntityKey { get; set; }
    public string? EntityName { get; set; }
  }
}
