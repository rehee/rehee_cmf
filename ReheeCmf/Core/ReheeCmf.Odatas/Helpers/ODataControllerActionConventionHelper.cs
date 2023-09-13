using Microsoft.AspNetCore.OData.Routing.Template;
using ReheeCmf.ODatas.Commons;
using ReheeCmf.ODatas.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ODatas.Helpers
{
  public static class ODataControllerActionConventionHelper
  {
    public static CmfConvention New(string prefix, string controllerName, string actionName,
      string path, string entityKey, string? entityName = null)
    {
      var paths = new EntitySetTemplateSegment(path, entityKey, entityName);
      return new CmfConvention(prefix, controllerName, actionName, new ODataPathTemplate(paths));
    }
  }
}
