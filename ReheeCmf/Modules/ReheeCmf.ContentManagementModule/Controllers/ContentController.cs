using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ReheeCmf.ContextModule.Entities;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ReheeCmf.ContentManagementModule.DTOs;
namespace ReheeCmf.ContentManagementModule.Controllers
{
  [Route("Api/Data/CmsContentDTO")]
  public class ContentController : ODataController
  {
    private readonly IContext context;
    static ScriptOptions? options { get; set; }
    public ContentController(IContext context)
    {
      this.context = context;
    }
    [EnableQuery]
    [HttpGet()]
    public async Task<IEnumerable<CmsContentDTO>> Query(CancellationToken ct)
    {

      var code = "x => !x.EmailConfirmed";
      options = options ?? ScriptOptions.Default
        .WithReferences(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic))
        .WithImports("System", "System.Linq", "System.Linq.Expressions");

      var d = await CSharpScript.EvaluateAsync(
        $$"""
        global::System.Linq.Expressions.Expression<System.Func<ReheeCmf.ContextModule.Entities.ReheeCmfBaseUser, System.Boolean>> exp = {{code}};
        return exp;
        """
        , options, cancellationToken: ct);

      var result = d as Expression<Func<ReheeCmfBaseUser, System.Boolean>>;


      var query = context.Query<ReheeCmfBaseUser>(true).Where(result);
      //context.Query<ReheeCmfBaseUser>(true).Where();

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
