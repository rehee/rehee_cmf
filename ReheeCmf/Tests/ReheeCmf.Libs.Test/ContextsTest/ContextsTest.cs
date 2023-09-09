using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Caches;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.ContextModule.Contexts;
using ReheeCmf.ContextModule.Interceptors;
using ReheeCmf.Contexts;
using ReheeCmf.Helpers;
using ReheeCmf.Libs.Test.ContextsTest.Contexts;
using ReheeCmf.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Libs.Test.ContextsTest
{
  internal abstract class ContextsTest<TDbContext> where TDbContext: DbContext
  {
    protected IServiceCollection? services { get; set; }
    protected IServiceProvider? serviceProvider { get; set; }
    public virtual void ConfigService()
    {
      services!.AddSingleton<CrudOption>(sp => new CrudOption
      {
        SQLType = Enums.EnumSQLType.Memory,
        DefaultConnectionString = "ssr"
      });
      services!.AddMemoryCache();
      services!.AddCmfMemoryCache<IContext, object, object>(-1, -1);
      services!.AddMemoryKeyValueCache<ICmfDbCommandInterceptor>(1);
      services!.AddScoped<ICmfDbCommandInterceptor, CmfDbCommandInterceptor>();
      services!.AddScoped<IContextScope<QuerySecondCache>, ContextScope<QuerySecondCache>>();
      services!.AddScoped<IContextScope<Tenant>, ContextScope<Tenant>>();
      services!.AddScoped<IContextScope<TokenDTO>, ContextScope<TokenDTO>>();
      services!.AddDbContext<TDbContext>();
      services!.AddScoped<IContext>(sp => new CmfRepositoryContext(sp, sp.GetService<TDbContext>()!));

    }

    [SetUp]
    public virtual void Setup()
    {
      EntityChangeHandlerFactory.Init();
      services = new ServiceCollection();
      ConfigService();
      serviceProvider = services.BuildServiceProvider();
    }
  }
}
