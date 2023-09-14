using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Caches;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.ContextModule;
using ReheeCmf.ContextModule.Contexts;
using ReheeCmf.ContextModule.Interceptors;
using ReheeCmf.Contexts;
using ReheeCmf.Helpers;
using ReheeCmf.Libs.Test.ContextsTest.Contexts;
using ReheeCmf.Servers.Services;
using ReheeCmf.Services;
using ReheeCmf.Tenants;
using ReheeCmf.Utility.CmfRegisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Test.ContextsTest
{
  internal abstract class ContextsTest<TDbContext> where TDbContext : DbContext
  {
    //protected IServiceCollection? services { get; set; }
    //protected IServiceProvider? serviceProvider { get; set; }
    public virtual IServiceProvider ConfigService(params Action<IServiceCollection>[] actions)
    {

      var services = new ServiceCollection();
      services!.AddSingleton<CrudOption>(sp => new CrudOption
      {
        SQLType = Enums.EnumSQLType.Memory,
        DefaultConnectionString = Guid.NewGuid().ToString(),
      });

      services!.AddMemoryCache();

      //services!.AddCmfMemoryCache<IContext, object, object>(-1, -1);
      services!.AddMemoryKeyValueCache<ICmfDbCommandInterceptor>(1);
      //services!.AddScoped<ICmfDbCommandInterceptor, CmfDbCommandInterceptor>();

      services!.AddScoped<IContextScope<QuerySecondCache>, ContextScope<QuerySecondCache>>();
      services!.AddScoped<IContextScope<Tenant>, ContextScope<Tenant>>();
      services!.AddScoped<IContextScope<TokenDTO>, ContextScope<TokenDTO>>();
      services!.AddDbContext<TDbContext>(a => { }, ServiceLifetime.Scoped);
      services!.AddScoped<IContext>(sp => new CmfRepositoryContext(sp, sp.GetService<TDbContext>()!));
      services!.AddScoped<ITenantStorage, TenantStorage>();
      services!.AddSingleton<IAsyncQuery, EFCoreAsyncQuery>();
      foreach (var a in actions)
      {
        a(services);
      }
      return services.BuildServiceProvider();
    }

    [SetUp]
    public virtual void Setup()
    {
      CmfRegister.Init();
      ContextModuleSetup.SetUpReflectPool(typeof(TDbContext));

      //services = new ServiceCollection();
      //ConfigService();
      //serviceProvider = services.BuildServiceProvider();
    }

  }
}
