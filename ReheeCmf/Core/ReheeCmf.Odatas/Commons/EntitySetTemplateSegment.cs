using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData.Edm;

namespace ReheeCmf.ODatas.Commons
{
  public class EntitySetTemplateSegment : ODataSegmentTemplate
  {
    private readonly string path;
    private readonly string entityName;
    private readonly string entityKey;

    public EntitySetTemplateSegment(string path, string entityKey, string entityName = null)
    {
      this.entityName = entityName;
      this.entityKey = entityKey;
      this.path = path;
    }
    public override IEnumerable<string> GetTemplates(ODataRouteOptions options)
    {
      yield return $"/{path}";
    }

    public override bool TryTranslate(ODataTemplateTranslateContext context)
    {
      string entitySetName = "";
      if (!string.IsNullOrEmpty(entityKey))
      {
        if (!context.RouteValues.TryGetValue(entityKey, out object classname))
        {
          return false;
        }
        entitySetName = classname as string;
      }
      if (!string.IsNullOrEmpty(entityName))
      {
        entitySetName = entityName;
      }
      // if you want to support case-insenstivie
      var edmEntitySet = context.Model.EntityContainer.EntitySets()
          .FirstOrDefault(e => string.Equals(entitySetName, e.Name, StringComparison.OrdinalIgnoreCase));

      //var edmEntitySet = context.Model.EntityContainer.FindEntitySet(entitySetName);
      if (edmEntitySet != null)
      {
        context.Segments.Add(new EntitySetSegment(edmEntitySet));
        return true;
      }

      return false;
    }
  }
}
