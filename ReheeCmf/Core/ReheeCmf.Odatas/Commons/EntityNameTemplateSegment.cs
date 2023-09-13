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
  public class EntityNameTemplateSegment : ODataSegmentTemplate
  {
    private readonly string setName;
    private readonly string existingPath;
    private readonly string? forcePath;

    public EntityNameTemplateSegment(string setName, string existingPath, string? forcePath = null)
    {
      this.setName = setName;
      this.existingPath = existingPath;
      this.forcePath = forcePath;
    }
    public override IEnumerable<string> GetTemplates(ODataRouteOptions options)
    {
      if (!String.IsNullOrEmpty(forcePath))
      {
        yield return forcePath;
      }
      else
      {
        yield return $"odata";
      }


    }

    public override bool TryTranslate(ODataTemplateTranslateContext context)
    {

      var edmEntitySet = context.Model.EntityContainer.EntitySets()
          .FirstOrDefault(e => string.Equals(setName, e.Name, StringComparison.OrdinalIgnoreCase));

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
