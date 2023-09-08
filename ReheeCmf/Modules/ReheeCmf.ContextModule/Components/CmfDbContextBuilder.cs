using ReheeCmf.ContextComponent;

namespace ReheeCmf.ContextModule.Components
{
  public class CmfDbContextBuilder : IDbContextBuilder
  {
    public void OnConfiguring(DbContextOptionsBuilder optionsBuilder, IContext? context)
    {
      optionsBuilder.UseInMemoryDatabase("mb");
    }
    public void OnModelCreating(ModelBuilder builder, IContext? context)
    {

    }
  }
}
