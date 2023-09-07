using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using OData.Swagger.Services;
using ReheeCmf.Authenticates;
using ReheeCmf.FileServices;
using ReheeCmf.Libs.Modules.Options;
using ReheeCmf.Requests;
using ReheeCmf.Servers.Filters;
using ReheeCmf.Servers.Services;
using ReheeCmf.Services;
using System.Net.Http;

namespace System
{
  public static class ReheeCmfServer
  {
    public static async Task WebStartUp<T>(string[] args) where T : ServiceModule, new()
    {

      var root = new T();
      var modules = ModuleHelper.GetAllService(root).ToArray();
      var builder = WebApplication.CreateBuilder(args);

      var context = new ServiceConfigurationContext
      {
        Services = builder.Services,
        Configuration = builder.Configuration,
        CrudOptions = builder.Configuration.GetOption<CrudOption>(),
        WebHost = builder.WebHost
      };
      var serverModule = modules.Select(b =>
      {
        if (b is ServiceModule s)
        {
          return s;
        }
        return null;
      }).Where(b => b != null).ToArray();
      foreach (var m in serverModule)
      {
        m.Constructor(context);
      }
      ModuleHelper.BlizorAssemblies = serverModule.SelectMany(b => b.BlazorAssemblies()).Distinct().ToArray();
      var configuration = context.Configuration;
      var services = context.Services;
      var tenent = context.Configuration.GetOption<TenantSetting>() ?? new TenantSetting();
      context.Tenant = tenent;
      context.Services.AddSingleton<TenantSetting>(tenent);

      services.AddScoped<IGetCurrentTenant, HttpRequestGetCurrentTenant>();
      services.AddSingleton<IIAsyncQuery, EFCoreAsyncQuery>();

      var options = configuration.GetOption<CrudOption>();
      services.AddSingleton<CrudOption>(options);
      var apiSetting = configuration.GetOption<ApiSetting>() ?? new ApiSetting();
      if (apiSetting.RSAOption == null || string.IsNullOrEmpty(apiSetting.RSAOption.RSAPrivateKey) || string.IsNullOrEmpty(apiSetting.RSAOption.RSAPublicKey))
      {
        apiSetting.RSAOption = new RSAOption();
        using (var ras = new RSACryptoServiceProvider())
        {
          apiSetting.RSAOption.RSAPrivateKey = ras.ToXmlString(true);
          apiSetting.RSAOption.RSAPublicKey = ras.ToXmlString(false);
        }
      }
      services.AddSingleton<ApiSetting>(apiSetting);
      services.AddHttpContextAccessor();
      services.AddHttpClient();
      services.AddScoped<IGetHttpClient, GetHttpClient>();
      services.AddRazorPages();

      var fileOption = configuration.GetOption<FileServiceOption>() ?? new FileServiceOption();
      services.AddSingleton<FileServiceOption>(fileOption);

      if (!tenent.CustomService)
      {
        if (!tenent.TenentContext)
        {
          context.Services.AddScoped<ITenantStorage, SettingTenantStorage>();
        }
        context.Services.AddScoped<ITenantService, TenantService>();
      }

      context.Services.AddScoped<IContextScope<Tenant>, ContextScope<Tenant>>();
      context.Services.AddScoped<IContextScope<TokenDTO>, ContextScope<TokenDTO>>();

      var serviceMapping = new Dictionary<string, Func<HttpClient>>();

      services.AddSingleton<IServiceModuleMapping, ServiceModuleMapping>();
      services.AddScoped<IToken<IServiceModuleRequestFactory>, TokenFromContextScope>();
      services.AddScoped<IServiceModuleRequestFactory, ServiceModuleRequestFactory>();

      context.MvcBuilder = context.Services.AddControllersWithViews(option =>
      {
        option.Filters.Add(typeof(CmfMultiTenancyFilter));
        option.Filters.Add(typeof(CmfAuthorizationFilter));
        option.Filters.Add(typeof(CmfExceptionFilter));

        foreach (var m in serverModule)
        {
          m.FiltersConfigration(option.Filters);
        }
      }).AddRazorOptions(options =>
      {
        options.AreaViewLocationFormats.Clear();
        options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
        options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
      });
      //TODO: Authorize model need refactory
      var authorizeOption = configuration.GetOption<AuthorizeOption>() ?? new AuthorizeOption();
      services.AddSingleton<AuthorizeOption>(authorizeOption);
      //services.AddSingleton<IToken<IAuthorize>, EmptyIAuthorizeToken>();
      //services.AddScoped<RequestWithFactory<IAuthorize>, AuthorizeRequest>(sp =>
      //{
      //  return new AuthorizeRequest(() =>
      //  {
      //    if (authorizeOption.IsDapr && !String.IsNullOrEmpty(authorizeOption.ServiceName))
      //    {
      //      return DaprClient.CreateInvokeHttpClient(authorizeOption.ServiceName);
      //    }
      //    var client = new HttpClient();
      //    if (!String.IsNullOrEmpty(authorizeOption.BaseUrl))
      //    {
      //      client.BaseAddress = new Uri(authorizeOption.BaseUrl);
      //    }
      //    return client;
      //  }, sp.GetService<IToken<IAuthorize>>(), sp.GetService<IContextScope<Tenant>>());
      //});
      services.AddScoped<IAuthorize, AuthorizeService>();
      //TODO
      services.AddScoped<IRequestClient<IAuthorize>, RequestClient<IAuthorize>>();
      services.AddSingleton<TokenValidationParameters>(sp => new TokenValidationParameters()
      {

      });

      foreach (var module in serverModule.Where(b => b != null))
      {
        foreach (var b2 in module!.EntityForContext())
        {
          ModuleOption.EntityInContextMap.AddOrUpdate(b2.Item1.Name, b2, (i, b) => b2);
        }
      }

      foreach (var m in serverModule)
      {
        await m.PreConfigureServicesAsync(context);
      }
      foreach (var m in serverModule)
      {
        m.JsonConfigration(context);
        await m.ConfigureServicesAsync(context);
      }
      services.AddCors();
      if (apiSetting.IsDapr)
      {
        context.MvcBuilder.AddControllersAsServices().AddDapr();
      }
      else
      {
        context.MvcBuilder.AddControllersAsServices();
      }



      //services.Replace(ServiceDescriptor.Transient<IControllerActivator, AutowiredControllerActivator>());

      var menuList = new List<NavActionItemDTO>();
      foreach (var m in modules)
      {
        var menuResponse = await m.GetMenu();
        if (!menuResponse.Success || menuResponse.Content?.Any() != true)
        {
          continue;
        }
        menuList.AddRange(menuResponse.Content);
      }
      ModuleOption.ModuleMenus = menuList.ToArray();
      foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        var name = assembly.GetName();
        ReflectPool.AssemblyPool.TryAdd(name.Name, assembly);
      }
      foreach (var exType in modules
        .Select(b => b.SharedEntityTypes)
        .SelectMany(b => b)
        .GroupBy(b => b)
        .Select(b => b.Key))
      {
        var name = exType.Name;

      }
      foreach (var m in modules.Select(b =>
      {
        if (b is ServiceModule sm)
        {
          return sm;
        }
        return null;
      }).Where(b => b != null))
      {
        ModuleOption.ServiceOnModelCreatingMap.AddOrUpdate(m.ModuleName, m.ServiceOnModelCreating, (i, b) => m.ServiceOnModelCreating);
        ModuleOption.ServiceOnConfiguringMap.AddOrUpdate(m.ModuleName, m.ServiceOnConfiguring, (i, b) => m.ServiceOnConfiguring);

      }
      //TODO: Need Implenent Shared Module
      //init SharedModuneType t
      //foreach (var exModule in modules
      //  .Select(b => b.SharedModuleTypes)
      //  .SelectMany(b => b)
      //  .DistinctBy(b => b.ModuleName))
      //{
      //  var sharedModule = exModule.GetAction()(null);
      //  ModuleOption.ExtureModuleMap.TryAdd(exModule.ModuleName, sharedModule);
      //}
      var reverseModule = serverModule.Reverse();

      services.AddScoped<IJWTService>(sp => default(IJWTService));
      services.AddScoped<TokenManagement>(sp => new TokenManagement());
      services.AddScoped<IGetRequestTokenService, GetRequestTokenService>();

      foreach (var m in reverseModule)
      {
        await m.PostConfigureServicesAsync(context);

      }
      foreach (var m in modules)
      {
        if (m.WithPermission)
        {
          services.AddSingleton(m.GetType(), m);
          ModuleOption.SetUpModule(m.ModuleName, m.GetType(), m.AddAssembly);
        }
      }

      Func<IApiVersionDescriptionProvider?> apiVersionDescriptionProvider = () =>
      {
        using (var scope = context.App.Services.CreateScope())
        {
          try
          {
            return scope.ServiceProvider.GetService<IApiVersionDescriptionProvider>();
          }
          catch
          {
            return null;
          }

        }
      };
      Func<ISwaggerApiVersion?> swaggerApiVersion = () =>
      {
        using (var scope = context.App.Services.CreateScope())
        {
          try
          {
            return scope.ServiceProvider.GetService<ISwaggerApiVersion>();
          }
          catch
          {
            return null;
          }

        }
      };
      services
        .AddSwaggerGen(c =>
        {
          var api = apiVersionDescriptionProvider();
          if (api == null)
          {
            root.SwaggerConfigration(c);
          }
          else
          {
            root.SwaggerConfigrationWithApiVersion(c, api, swaggerApiVersion());
          }

        });

      services.AddOdataSwaggerSupport();
      foreach (var s in serverModule.Where(b => b.RuningInFinal && b.GetType() != root.GetType()))
      {
        await s.FinalRootConfigure(context);
      }
      await root.FinalRootConfigure(context);


      var app = builder.Build();
      var env = app.Environment;
      context.App = app;
      context.Env = env;
      app.UseStaticFiles();

      app.UseAuthentication();
      app.UseAuthorization();


      var mapper = new Dictionary<string, string>();


      foreach (var s in serverModule)
      {
        await s.PreApplicationInitializationAsync(context);
      }
      app.UseRouting();
      foreach (var s in serverModule)
      {
        await s.ApplicationInitializationAsync(context);
      }



      _ = app.UseEndpoints(endpoints =>
      {
        foreach (var m in serverModule)
        {
          m.EndpointRouteBuilder(endpoints, context);
        }
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
      });

      foreach (var s in reverseModule)
      {
        await s.PostApplicationInitializationAsync(context);
      }

      app.Run();

    }
  }
}
