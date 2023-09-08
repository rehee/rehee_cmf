using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReheeCmf;
using ReheeCmf.ContextComponent;
using ReheeCmf.ContextModule.Contexts;
using ReheeCmf.Contexts;
using ReheeCmf.Entities;

namespace CmfDemo.Data
{
  [ComponentHandler<MyHandler>(Index = 1)]
  public class ApplicationDbContext : CmfIdentityContext<IdentityUser>
  {
    public ApplicationDbContext(IServiceProvider sp) : base(sp)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
    }

    public DbSet<Entity1> Entity1s { get; set; }
  }

  public class Entity1 : EntityBase<int>
  {

  }

  public class MyHandler : IDbContextBuilder
  {
    public  void OnConfiguring(DbContextOptionsBuilder optionsBuilder, IContext? context)
    {

    }
    public  void OnModelCreating(ModelBuilder builder, IContext? context)
    {
    }
  }
}
