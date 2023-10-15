using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis.Scripting;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.CodeAnalyses;
using ReheeCmf.ContentManagementModule.CodeAnalyses;
using ReheeCmf.Storages;
namespace ReheeCmf.ContentManagementModule.Controllers
{
  [Route("Api/Content")]
  public class ContentController : ODataController
  {
    private readonly IContext context;
    private readonly ICmfPredicateExpression<ContentManagementExpressOption> analysisService;
    private readonly IStorage<CmsEntityMetadata, Guid> entityStorage;

    static ScriptOptions? options { get; set; }
    public ContentController(IContext context, ICmfPredicateExpression<ContentManagementExpressOption> analysisService, IStorage<CmsEntityMetadata, Guid> entityStorage)
    {
      this.context = context;
      this.analysisService = analysisService;
      this.entityStorage = entityStorage;
    }
    [EnableQuery]
    [HttpGet("{entityName}/Json")]
    public async Task<IEnumerable<CmsContentDTO>> Query(string entityName, CancellationToken ct)
    {
      var entityMetaQuery = await entityStorage.GetQueryAsync(ct);
      var metaDataFind = entityMetaQuery.Where(b => b.EntityName == entityName).FirstOrDefault();
      var metaData =
        (from entityMeta in context.Query<CmsEntityMetadata>(true)
         let propertyMeta = context.Query<CmsPropertyMetadata>(true).Where(b => b.CmsEntityMetadataId == entityMeta.Id).Select(b => new { b.Id, b.PropertyType, b.PropertyName }).AsEnumerable()

         select new
         {
           entityMeta = entityMeta,
           propertyMeta = propertyMeta
         }).ToArray();
      var metaIds = metaData.Select(b => (Guid?)b.entityMeta.Id).ToArray();
      //var metas = metaData.SelectMany(b => b.propertyMeta).ToArray().AsEnumerable();
      var metas = context.Query<CmsPropertyMetadata>(true).AsEnumerable();
      return (
        from entity in context.Query<CmsEntity>(true).Where(b => metaIds.Contains(b.CmsEntityMetadataId))
        let properties = context.Query<CmsProperty>(true).Where(b => b.CmsEntityId == entity.Id).AsEnumerable()
        let properyItems = properties.Select(b => new CmsPropertyDTO
        {
          PropertyName = b.PropertyName,
          PropertyType = b.PropertyType,
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
        }).AsEnumerable()
        let items = properyItems
          .Select(b => new KeyValuePair<string, object?>(b.PropertyName,
            b.PropertyType == Enums.EnumPropertyType.Boolean ? b.ValueBoolean :
            b.PropertyType == Enums.EnumPropertyType.Int16 ? b.ValueInt16 :
            b.PropertyType == Enums.EnumPropertyType.Int32 ? b.ValueInt32 :
            b.PropertyType == Enums.EnumPropertyType.Int64 ? b.ValueInt64 :
            b.PropertyType == Enums.EnumPropertyType.Single ? b.ValueSingle :
            b.PropertyType == Enums.EnumPropertyType.Double ? b.ValueDouble :
            b.PropertyType == Enums.EnumPropertyType.Decimal ? b.ValueDecimal :
            b.PropertyType == Enums.EnumPropertyType.Guid ? b.ValueGuid :
            b.PropertyType == Enums.EnumPropertyType.DateTime ? b.ValueDateTime :
            b.PropertyType == Enums.EnumPropertyType.DateTimeOffset ? b.ValueDateTimeOffset :
          b.ValueString
          )).AsEnumerable()
        select new CmsContentDTO
        {
          Id = entity.Id,
          Data = new Dictionary<string, object?>(items),
          Properties = properyItems
        }
        ).AsSplitQuery();
    }
  }
}
