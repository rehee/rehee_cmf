using ReheeCmf.Commons;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Contexts;
using ReheeCmf.Enums;
using ReheeCmf.Modules;
using ReheeCmf.ODatas;
using ReheeCmf.ODatas.Commons;
using ReheeCmf.ODatas.Helpers;
using ReheeCmf.Reflects.ReflectPools;
using ReheeCmf.Helpers;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData;
using ReheeCmf.ODatas.Converters;
using Microsoft.AspNetCore.OData.Formatter.Serialization;

namespace ReheeCmf.EntityModule
{
  public class CmfEntityModule : ServiceModule
  {
    public override string ModuleTitle => ConstModule.CmfEntityModule;

    public override string ModuleName => ConstModule.CmfEntityModule;

    public override Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {

      var result = ReflectPool.EntityMapping_2.Select(b => b.Key.Name).SelectMany(b =>
        new string[]
        {
          EnumHttpMethod.Get.GetEntityPermission(b),
          EnumHttpMethod.Post.GetEntityPermission(b),
          EnumHttpMethod.Put.GetEntityPermission(b),
          EnumHttpMethod.Delete.GetEntityPermission(b),
        });
      return Task.FromResult(result);
    }

    public override async Task PostConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.PostConfigureServicesAsync(context);
      context.MvcBuilder!.AddCmfOdataEndpoint(sp =>
      {
        var m = sp.GetEdmModel(
          builder =>
          {
          }, context.CrudOptions!.UserType!, context.CrudOptions!.RoleType!, context.CrudOptions);
        ReflectPool.OdataEdmModelEntity = m;
        return m;
      }
    , CrudOption.DataApiEndpoint, new ODataEndpointMapping[] {
          ODataEndpointMapping.New("DataApiController", "Query", "{entityName}", "entityName"),
          ODataEndpointMapping.New("DataApiController", "FindEntity", CrudOption.DataEndpoint, "entityName"),
    });
      context.MvcBuilder.AddOData((opt, sp) =>
      {
        if (ODataPools.QueryNameBuilderMapping?.Any() == true)
        {
          var builder = new ODataConventionModelBuilder();
          var entitySet = typeof(ODataConventionModelBuilder).GetMethod("EntitySet");
          foreach (var m in ODataPools.QueryNameBuilderMapping)
          {
            var set = entitySet.MakeGenericMethod(
              ODataPools.QueryNameKeyTypeMapping.GetValueOrDefault(m.Key)).Invoke(builder, new object[] { m.Key }).GetPropertyValue("EntityType");
            m.Value.Invoke("Invoke", set.Content);
          }
          var edmModel = builder.GetEdmModel();
          opt.AddRouteComponents(CrudOption.DTOProcessor, edmModel, action =>
          {
            action.AddSingleton<ODataPayloadValueConverter, ReheeCMFOdataConverter>();
            action.AddSingleton<IODataSerializerProvider, ReheeCmfETagSerializerProvider>();
          });
          opt.Conventions.Add(ODataControllerActionConventionHelper.New(
          CrudOption.DTOProcessor, "DTOProcessorController", "Query", "{dtoName}", "dtoName"));
          opt.Conventions.Add(ODataControllerActionConventionHelper.New(
          CrudOption.DTOProcessor, "DTOProcessorController", "Find", "{dtoName}/{queryKey}", "dtoName"));
          ReflectPool.OdataEdmModelQueryDTO = edmModel;
        }
      });

    }
  }
}
