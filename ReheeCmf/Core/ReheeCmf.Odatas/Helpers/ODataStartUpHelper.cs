using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ReheeCmf.Commons;
using ReheeCmf.Helpers;
using ReheeCmf.ODatas.Components;
using ReheeCmf.Reflects.ReflectPools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ODatas.Helpers
{
  public static class ODataStartUpHelper
  {
    public static IEndpointRouteBuilder ODataController(this IEndpointRouteBuilder endpoints, IServiceProvider sp)
    {
      return endpoints;
    }
    public static IEdmModel GetEdmModel(this Action<ODataConventionModelBuilder> buildAction)
    {
      var builder = new ODataConventionModelBuilder();
      buildAction(builder);
      return builder.GetEdmModel();
    }
    public static IEdmModel GetEdmModel(this IServiceProvider sp, Action<ODataConventionModelBuilder> BildEntity, Type userType, Type roleType, CrudOption option = null, bool isPublic = false)
    {
      var builder = new ODataConventionModelBuilder();
      var entityList = ReflectPool.EntityMapping_2.Select(b => b.Key).ToArray();
      var mapping = typeof(ODataModelBuilder).GetMap();
      var method = mapping.Methods.FirstOrDefault(b => b.Name == "EntitySet" && b.GetParameters().Length == 1);
      if (entityList.Length <= 0)
      {
        return builder.GetEdmModel();
      }


      foreach (var entity in entityList)
      {
        var handler = ODataEntitySetFactory.GetHandler(entity);
        if (handler != null)
        {
          ODataEntitySetExtensions.SetODataMapping(entity, handler.EntitySet(builder));
        }
        else
        {
          var invoke = method.MakeGenericMethod(entity);
          invoke.Invoke(builder, new object[] { entity.Name });
          var entityTypeMethod = mapping.Methods.FirstOrDefault(b => b.Name == "EntityType");
          var invoke2 = entityTypeMethod.MakeGenericMethod(entity);
          var entityType = invoke2.Invoke(builder, null);
          ODataEntitySetExtensions.SetODataMapping(entity, entityType);
        }
      }
      if (BildEntity != null)
      {
        BildEntity(builder);
      }
      return builder.GetEdmModel();
    }
    public static IEdmModel GetEdmModelUser(this IServiceProvider sp, Action<ODataConventionModelBuilder> BuildUser, Type userType = null, Type roleType = null)
    {
      var builder = new ODataConventionModelBuilder();
      //var factory = sp.GetService(typeof(IContextFactory)) as IContextFactory;
      var options = sp.GetService<CrudOption>() ?? new CrudOption();
      var entityList = ReflectPool.EntityMapping_2.Select(b => b.Key).ToArray();
      var mapping = typeof(ODataModelBuilder).GetMap();
      var method = mapping.Methods.FirstOrDefault(b => b.Name == "EntitySet" && b.GetParameters().Length == 1);
      if (entityList?.Length <= 0)
      {
        return builder.GetEdmModel();
      }
      var roleEntity = entityList!.FirstOrDefault(b => b.Name == options.EntityKey_IdentityRole);
      var invokeRole = method.MakeGenericMethod(roleEntity!);
      invokeRole.Invoke(builder, new object[] { CrudOption.RoleEndpoint });

      if (BuildUser != null)
      {
        BuildUser(builder);
      }
      return builder.GetEdmModel();
    }
  }
}
