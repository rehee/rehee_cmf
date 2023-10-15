using ReheeCmf.Commons.DTOs;
using ReheeCmf.Modules;
using ReheeCmf.ODatas.Commons;
using ReheeCmf.ODatas;
using Microsoft.OData.ModelBuilder;
using ReheeCmf.CmfCodeAnalyses;
using ReheeCmf.CodeAnalyses;
using ReheeCmf.ContentManagementModule.CodeAnalyses;
namespace ReheeCmf.ContentManagementModule
{
  public class CmfContentManagememntModule : ServiceModule
  {
    public override string ModuleTitle => nameof(CmfContentManagememntModule);

    public override string ModuleName => nameof(CmfContentManagememntModule);

    public override async Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {
      await Task.CompletedTask;
      return [];
    }

    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.ConfigureServicesAsync(context);
      context.Services!.AddCmfPredicateExpression<ContentManagementExpressOption>(
        sp => new CmfCodeAnalysisOption<ContentManagementExpressOption>
        {
          Template = ContentManagementExpressOption.QueryPredicateLambdaTemplate
        });
      context.Services!.AddCmfStorageSetup<CmsEntityMetadata, Guid>();
    }

    public override async Task PostConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.PostConfigureServicesAsync(context);
      context.MvcBuilder!.AddCmfOdataEndpoint(sp =>
      {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<CmsContentDTO>("CmsContentDTO");
        return builder.GetEdmModel();
      }
    , "Api/Content", [
          ODataEndpointMapping.New("ContentController", "Query", "{entityName}", "entityName", "CmsContentDTO"),
      //ODataEndpointMapping.New("DataApiController", "FindEntity", CrudOption.DataEndpoint, "entityName"),
    ]);
    }
  }
}
