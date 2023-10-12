using Microsoft.Extensions.Logging;
using ReheeCmf.Caches.MemoryCaches;
using ReheeCmf.ContextComponent;
using ReheeCmf.ContextModule.Interceptors;
using ReheeCmf.Enums;
using ReheeCmf.Utility.CmfRegisters;
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
      var logService = sp.GetService<ILogger<DbContext>>();
      if (logService != null)
      {
        optionsBuilder.LogTo(s => logService.Log(LogLevel.Information, s), LogLevel.Information);
      }
      else
      {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
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
      if (context is ITenantContext tenantContext)
      {
        foreach (var handler in CmfRegister.RegistrableEntityPool.Values)
        {
          handler.RegisterEntity(builder, tenantContext);
        }
      }
    }

  }
}
