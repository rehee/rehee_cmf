using CmfDemo.Data;
using Microsoft.AspNetCore.Identity;
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
using ReheeCmf.ODatas;
using ReheeCmf.Services;

namespace CmfDemo
{
  public class DemoModule : CmfApiModule
  {
    public override IEnumerable<ModuleDependOn> Depends()
    {
      return base.Depends().Concat(new ModuleDependOn[]
      {
        ModuleDependOn.New<CmfContextModule<ApplicationDbContext,ReheeCmfBaseUser>>(),
        ModuleDependOn.New<CmfEntityModule>(),
        ModuleDependOn.New<CmfEntityModule>()
      });
    }
    public override string ModuleTitle => "";

    public override string ModuleName => "";


    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.ConfigureServicesAsync(context);
      context.Services.AddTypeQuery<Entity1DTO, Entity1DTOProcessor>();


      context.MvcBuilder.AddCmfOData();

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
