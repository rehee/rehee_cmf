using CmfDemo.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ReheeCmf;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.ContextModule;
using ReheeCmf.ContextModule.Contexts;
using ReheeCmf.ContextModule.Entities;
using ReheeCmf.Contexts;
using ReheeCmf.DTOProcessors;
using ReheeCmf.DTOProcessors.Processors;
using ReheeCmf.EntityModule;
using ReheeCmf.Modules;
using ReheeCmf.Modules.Helpers;
using ReheeCmf.Modules.Permissions;
using ReheeCmf.ODatas;
using ReheeCmf.Services;
using ReheeCmf.UserManagementModule;

namespace CmfDemo
{
  public class DemoModule : CmfApiModule<ApplicationDbContext, ReheeCmfBaseUser>
  {
    public override string ModuleTitle => "";
    public override string ModuleName => "";


    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.ConfigureServicesAsync(context);
      context.Services.AddTypeQuery<Entity1DTO, Entity1DTOProcessor>();
    }
    public override async Task ApplicationInitializationAsync(ServiceConfigurationContext context)
    {
      await base.ApplicationInitializationAsync(context);

      //options.EntityQueryUri ?? $"{current.Request.Scheme}://{current.Request.Host}{current.Request.PathBase}";
    }
    public override Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {
      return Task.FromResult(Array.Empty<string>().AsEnumerable());
    }
  }

  public class Entity1DTO : CmfDTOBase
  {
    public string? Name1 { get; set; }
    public string? Name2 { get; set; }
  }
  public class Entity1DTOProcessor : DtoProcessor<Entity1DTO>
  {
    public Entity1DTOProcessor(IContext context, IAsyncQuery asyncQuery) : base(context, asyncQuery)
    {
    }
    public override IQueryable<Entity1DTO> QueryWithType(TokenDTO user)
    {
      return context.Query<Entity1>(true).Select(b => new Entity1DTO
      {
        QueryKey = b.Id.ToString(),
        Name1 = b.Name1,
        Name2 = b.Name2
      });
    }
  }
}
