using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis.Scripting;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.CodeAnalyses;
using ReheeCmf.ContentManagementModule.CodeAnalyses;
namespace ReheeCmf.ContentManagementModule.Controllers
{
  [Route("Api/Content")]
  public class ContentController : ODataController
  {
    private readonly IContext context;
    private readonly ICmfPredicateExpression<ContentManagementExpressOption> analysisService;

    static ScriptOptions? options { get; set; }
    public ContentController(IContext context, ICmfPredicateExpression<ContentManagementExpressOption> analysisService)
    {
      this.context = context;
      this.analysisService = analysisService;
    }
    [EnableQuery]
    [HttpGet("{entityName}/Json")]
    public async Task<IEnumerable<CmsContentDTO>> Query(string entityName, CancellationToken ct)
    {
      var exp = await analysisService.TypedEvaluateAsync<TokenDTO, TokenDTO>(ct, "x=>true");


      var c1 = exp!(null);
      var c2 = c1.Compile()(null);
      var metaData =
        (from entityMeta in context.Query<CmsEntityMetadata>(true)
         join propertyMeta in context.Query<CmsPropertyMetadata>(true) on entityMeta.Id equals propertyMeta.CmsEntityMetadataId into gp
         select new
         {
           entityMeta
         }).ToArray();
      var metaIds = metaData.Select(b => (Guid?)b.entityMeta.Id).ToArray();
      return
        from entity in context.Query<CmsEntity>(true).Where(b => metaIds.Contains(b.CmsEntityMetadataId))
        let properties = context.Query<CmsProperty>(true).Where(b => b.CmsEntityId == entity.Id).AsEnumerable()
        let properyItems = properties.Select(b => new CmsPropertyDTO
        {
          PropertyName = b.Property.PropertyName,
          ValueBoolean = b.ValueBoolean,
          ValueInt16 = b.ValueInt16,
          ValueInt32 = b.ValueInt32,
          ValueInt64 = b.ValueInt64,
          ValueString = b.ValueString,
          ValueSingle = b.ValueSingle,
          ValueDouble = b.ValueDouble,
          ValueDecimal = b.ValueDecimal,
          ValueGuid = b.ValueGuid,
          ValueDateTime = b.ValueDateTime,
          ValueDateTimeOffset = b.ValueDateTimeOffset,
        })
        let items = properties
          .Select(b => new KeyValuePair<string, object?>(b.Property.PropertyName,
          b.Property.PropertyType == Enums.EnumPropertyType.Boolean ? b.ValueBoolean :
            b.Property.PropertyType == Enums.EnumPropertyType.Int16 ? b.ValueInt16 :
            b.Property.PropertyType == Enums.EnumPropertyType.Int32 ? b.ValueInt32 :
            b.Property.PropertyType == Enums.EnumPropertyType.Int64 ? b.ValueInt64 :
            b.Property.PropertyType == Enums.EnumPropertyType.Single ? b.ValueSingle :
            b.Property.PropertyType == Enums.EnumPropertyType.Double ? b.ValueDouble :
            b.Property.PropertyType == Enums.EnumPropertyType.Decimal ? b.ValueDecimal :
            b.Property.PropertyType == Enums.EnumPropertyType.Guid ? b.ValueGuid :
            b.Property.PropertyType == Enums.EnumPropertyType.DateTime ? b.ValueDateTime :
            b.Property.PropertyType == Enums.EnumPropertyType.DateTimeOffset ? b.ValueDateTimeOffset :
          b.ValueString
          ))
        select new CmsContentDTO
        {
          Id = entity.Id,
          Data = new Dictionary<string, object?>(items),
          Properties = properyItems
        };
    }
  }
}
