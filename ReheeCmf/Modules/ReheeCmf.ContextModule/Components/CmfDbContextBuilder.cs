using Microsoft.Extensions.Caching.Memory;
using ReheeCmf.Caches.MemoryCaches;
using ReheeCmf.Components;
using ReheeCmf.ContextComponent;
using ReheeCmf.ContextModule.Interceptors;
using ReheeCmf.Enums;

namespace ReheeCmf.ContextModule.Components
{
  public class CmfDbContextBuilder : IDbContextBuilder
  {
    public virtual void OnConfiguring(DbContextOptionsBuilder optionsBuilder, IServiceProvider sp, DbContext context)
    {
      var option = sp.GetService<CrudOption>()!;
      var mc = sp.GetService<ITypedMemoryCache<IContext>>();
      var interceptor = sp.GetService<ICmfDbCommandInterceptor>();
      if (mc != null)
      {
        optionsBuilder.UseMemoryCache(mc);
      }
      if (!option.SQLType.HasValue)
      {
        optionsBuilder.UseSqlServer(option.DefaultConnectionString);
      }
      else
      {
        switch (option.SQLType.Value)
        {
          case EnumSQLType.Memory:
            optionsBuilder.UseInMemoryDatabase(databaseName: option.DefaultConnectionString);
            break;
          case EnumSQLType.PGSql:
            optionsBuilder.UseNpgsql(option.DefaultConnectionString);
            break;
          case EnumSQLType.SQLite:
            optionsBuilder.UseSqlite(option.DefaultConnectionString);
            break;
          default:
            optionsBuilder.UseSqlServer(option.DefaultConnectionString);
            break;
        }
      }
      if (interceptor != null)
      {
        optionsBuilder.AddInterceptors(interceptor);
      }
      if (!option.DisableUseLazyLoadingProxies)
      {
        optionsBuilder.UseLazyLoadingProxies(true);
      }
    }

    public virtual void OnModelCreating(ModelBuilder builder, IServiceProvider sp, DbContext context)
    {
      
    }

  }
}
