using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.Commons.Interfaces;
using ReheeCmf.ContextModule.Contexts;
using ReheeCmf.Entities;
using ReheeCmf.Handlers.EntityChangeHandlers;

namespace CmfDemo.Data
{
  public class ApplicationDbContext : CmfIdentityContext<IdentityUser>
  {
    public ApplicationDbContext(IServiceProvider sp) : base(sp)
    {

    }


    public DbSet<Entity1> Entity1s { get; set; }
  }
  //, IWithName
  public class Entity1 : EntityBase<int>
  {
    public string? Name1 { get; set; }
    public string? Name2 { get; set; }
    
  }

  [EntityChangeTracker<Entity1, Entity1Tracker>]
  public class Entity1Tracker : EntityChangeHandler<Entity1>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      entity.Name1 = Guid.NewGuid().ToString();

    }
  }

}
