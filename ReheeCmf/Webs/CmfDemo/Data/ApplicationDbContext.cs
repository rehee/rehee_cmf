using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.Attributes;
using ReheeCmf.Commons.Interfaces;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.ContextModule.Contexts;
using ReheeCmf.ContextModule.Entities;
using ReheeCmf.Entities;
using ReheeCmf.Handlers.EntityChangeHandlers;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmfDemo.Data
{
  public class ApplicationDbContext : CmfIdentityContext<ReheeCmfBaseUser>
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
    [NotMapped]
    public DateTime? Date1 { get; set; }

    [ReadCheck]
    public static ReadCheck<Entity1> Entity1ReadCheck = (user) => b => b.Id > 20;
  }

  [EntityChangeTracker<Entity1>]
  public class Entity1Tracker : EntityChangeHandler<Entity1>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      entity.Name1 = Guid.NewGuid().ToString();

    }
  }

}
