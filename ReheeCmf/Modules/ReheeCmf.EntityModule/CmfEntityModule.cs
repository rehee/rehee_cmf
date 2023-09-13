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
namespace ReheeCmf.EntityModule
{
  public class CmfEntityModule : ServiceModule
  {
    public override string ModuleTitle => nameof(CmfEntityModule);

    public override string ModuleName => nameof(CmfEntityModule);

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
      context.MvcBuilder.AddCmfOdataEndpoint(sp =>
      {
        var m = sp.GetEdmModel(
          builder =>
          {
          }, context.CrudOptions.UserType, context.CrudOptions.RoleType, context.CrudOptions);
        ReflectPool.OdataEdmModelEntity = m;
        return m;
      }
    , CrudOption.DataApiEndpoint, new ODataEndpointMapping[] {
          ODataEndpointMapping.New("DataApiController", "Query", "{entityName}", "entityName"),
          ODataEndpointMapping.New("DataApiController", "FindEntity", CrudOption.DataEndpoint, "entityName"),
    });
    }
  }
}
