using ReheeCmf.ConstValues;
using ReheeCmf.Contexts;
using ReheeCmf.Enums;
using ReheeCmf.Modules;
using ReheeCmf.Reflects.ReflectPools;
using ReheeCmf.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContextModule
{
  public class CmfContextModule<TContext, TUser>
    : ServiceModule
    where TContext : DbContext
    where TUser : IdentityUser, ICmfUser, new()
  {
    public override string ModuleTitle => "CmfContextModule";
    public override string ModuleName => "CmfContextModule";
    

    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      context.Services!.AddContextModule<TContext, TUser>(context.Configuration!);
      return Task.CompletedTask;
    }

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
  }
}
