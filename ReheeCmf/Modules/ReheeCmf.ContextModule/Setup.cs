using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReheeCmf.Attributes;
using ReheeCmf.Authenticates;
using ReheeCmf.Caches;
using ReheeCmf.Commons;
using ReheeCmf.ContextModule.Contexts;
using ReheeCmf.ContextModule.Interceptors;
using ReheeCmf.ContextModule.Managers;
using ReheeCmf.ContextModule.Providers;
using ReheeCmf.Handlers.ContextHandlers;
using ReheeCmf.Helper;
using ReheeCmf.Helpers;
using ReheeCmf.Modules.Options;

using ReheeCmf.Reflects.ReflectPools;
using System.ComponentModel.DataAnnotations;

namespace ReheeCmf.ContextModule
{
  public static class ContextModuleSetup
  {
    public static IServiceCollection AddContextModule<TContext, TUser>(
      this IServiceCollection services, IConfiguration configuration)
      where TContext : DbContext
      where TUser : IdentityUser, new()
    {
      var option = configuration.GetOption<CrudOption>();
      option.EntityKey_IdentityUserToken = typeof(TUser).Name;
      option.UserType = typeof(TUser);
      option.RoleType = typeof(TenantIdentityRole);
      option.DefaultConnectionString = configuration.GetConnectionString("DefaultConnection")!;
      services.AddSingleton<CrudOption>(option);
      var userManagement = configuration.GetOption<UserManagementOption>(defaultValue: UserManagementOption.Detault);
      services.AddSingleton<UserManagementOption>(userManagement);
      services.AddMemoryCache();
      services.AddCmfMemoryCache<IContext, object, object>(-1, -1);
      services.AddScoped<ICmfDbCommandInterceptor, CmfDbCommandInterceptor>();
      services.AddMemoryKeyValueCache<ICmfDbCommandInterceptor>(1);
      services.AddDbContext<TContext>();

      var contextFactory = typeof(TContext).GetComponentsByHandler<IContextFactoryHandler>();
      if (contextFactory?.Any() == true)
      {
        var fac = contextFactory.FirstOrDefault()!.SingletonHandler<IContextFactoryHandler>()!;
        services.AddScoped<IContext>(sp => fac.CreateContext(sp));
      }
      else
      {
        services.AddScoped<IContext>(sp =>
          new CmfRepositoryContext(sp, sp.GetService<TContext>()!));
      }
      services.AddScoped<ITenantStorage, TenantStorage>();

      var contextType = typeof(TContext);
      if (contextType.IsImplement<IIdentityContext>())
      {
        var tokenProvider = typeof(ReheeCmfDefaultAuthenticationProvider<TUser>);
        option.RoleType = typeof(TenantIdentityRole);
        option.EntityKey_IdentityRole = typeof(TenantIdentityRole).Name;
        services.AddIdentity<TUser, TenantIdentityRole>(
          options => options.SignIn.RequireConfirmedAccount = false)
          .AddEntityFrameworkStores<TContext>()
          .AddTokenProvider("Default", tokenProvider);

        services.AddScoped<RoleManager<TenantIdentityRole>>();

        services.AddScoped<UserManager<TUser>>();
        services.AddScoped<SignInManager<TUser>, TenantSignInManager<TUser>>();

        if (typeof(TUser) != typeof(IdentityUser))
        {
          services.AddDefaultUser<IdentityUser>(sp =>
            sp.GetService<TContext>()!);
        }


      }

      services.AddScoped<IDTOSignInManager<LoginDTO>, LoginDTOSignInManager<TUser>>();
      services.AddScoped<IDTOSignInManager<ClaimsPrincipalDTO>, ClaimsPrincipalSignInManager<TUser>>();
      SetUpReflectPool(typeof(TContext));

      return services;
    }
    public static IServiceCollection AddDefaultUser<TUser>(this IServiceCollection services, Func<IServiceProvider, DbContext> dbContext)
      where TUser : IdentityUser, new()
    {
      services.AddScoped<IUserStore<TUser>>(sp =>
          new UserStore<TUser>(dbContext(sp), sp.GetService<IdentityErrorDescriber>()));
      services.AddScoped<IPasswordHasher<TUser>>(sp =>
        new PasswordHasher<TUser>(sp.GetService<IOptions<PasswordHasherOptions>>()));
      services.AddScoped<IEnumerable<IUserValidator<TUser>>>(sp =>
       new IUserValidator<TUser>[] {
           new UserValidator<TUser>(sp.GetService<IdentityErrorDescriber>())
        }
      );
      services.AddScoped<IEnumerable<IPasswordValidator<TUser>>>(sp =>
        new IPasswordValidator<TUser>[] {
                 new PasswordValidator<TUser>(sp.GetService<IdentityErrorDescriber>())
         }
      );

      services.AddScoped<UserManager<TUser>>(sp =>
       new UserManager<TUser>(
         sp.GetService<IUserStore<TUser>>()!,
         sp.GetService<IOptions<IdentityOptions>>()!,
         sp.GetService<IPasswordHasher<TUser>>()!,
         sp.GetService<IEnumerable<IUserValidator<TUser>>>()!,
         sp.GetService<IEnumerable<IPasswordValidator<TUser>>>()!,
         sp.GetService<ILookupNormalizer>()!,
         sp.GetService<IdentityErrorDescriber>()!,
         sp,
         sp.GetService<ILogger<UserManager<TUser>>>()!
         ));

      services.AddScoped<IUserClaimsPrincipalFactory<TUser>>(sp =>
        new UserClaimsPrincipalFactory<TUser>(
           sp.GetService<UserManager<TUser>>()!,
           sp.GetService<IOptions<IdentityOptions>>()!
          ));
      services.AddScoped<IUserConfirmation<TUser>>(sp =>
       new DefaultUserConfirmation<TUser>());
      services.AddScoped<SignInManager<TUser>>(sp =>
        new TenantSignInManager<TUser>(
          sp.GetService<IContext>()!,
          sp.GetService<UserManager<TUser>>()!,
          sp.GetService<IHttpContextAccessor>()!,
          sp.GetService<IUserClaimsPrincipalFactory<TUser>>()!,
          sp.GetService<IOptions<IdentityOptions>>()!,
          sp.GetService<ILogger<SignInManager<TUser>>>()!,
          sp.GetService<IAuthenticationSchemeProvider>()!,
          sp.GetService<IUserConfirmation<TUser>>()!
          ));


      return services;
    }
    public static void SetUpReflectPool(Type dbType)
    {
      var properties = dbType.GetMap().Properties
        .Where(b => b.PropertyType.IsIEnumerable())
        .Where(b => b.PropertyType.IsGenericType &&
          b.PropertyType.GenericTypeArguments.Length == 1)
        .Select(p =>
        {
          var type = p.PropertyType.GenericTypeArguments.FirstOrDefault();
          return (
          EntityType: type,
          TypeName: type.Name,
          SetName: p.Name
          );
        })
        .ToArray();
      var entityForContext = ModuleOption.EntityInContextMap.Select(b =>
      {
        return (
          EntityType: b.Value.Item1,
          TypeName: b.Key,
          SetName: b.Value.Item2
          );
      });

      var allProperties = properties.Concat(entityForContext);
      //var setMethod = dbType.GetMap().Methods.Where(b => b.Name == "Set" && b.GetParameters().Length == 0).FirstOrDefault();

      foreach (var p in allProperties)
      {
        ReflectPool.EntityNameMapping.TryAdd(p.TypeName, p.EntityType);
        ReflectPool.SetQueryAndFindCheck(p.EntityType);
        var typeMap = p.EntityType.GetMap();
        var id = typeMap.Properties.Where(b =>
          b.HasCustomAttribute<KeyAttribute>() ||
          b.HasCustomAttribute(nameof(PersonalDataAttribute)) ||
          b.Name == nameof(IId<int>.Id)).FirstOrDefault();
        ReflectPool.EntityKeyMapping.AddOrUpdate(p.EntityType, id, (c, b) => id);

        var queryFirstProperty = typeMap.Properties.Where(b => b.HasCustomAttribute<QueryBeforeFilterAttribute>()).Select(b => b.Name).ToArray();
        ReflectPool.EntityTypeQueryFirst.TryAdd(p.EntityType, queryFirstProperty);
        ReflectPool.EntityMapping_2.TryAdd(p.EntityType, p.SetName);
      }


      ReflectPool.AsNoTracking = typeof(EntityFrameworkQueryableExtensions).GetMethods().Where(b => b.Name.Equals("AsNoTracking")).FirstOrDefault()!;

    }
  }
}
