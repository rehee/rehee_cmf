﻿
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Authenticates;
using ReheeCmf.Helpers;
using ReheeCmf.Modules.ApiVersions;
using ReheeCmf.Tenants;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ReheeCmf.Modules
{

  public abstract class ServiceModule
  {
    public virtual bool FlagOnly => false;
    public Type[]? GenericTypeParameters { get; set; }
    public virtual void SetGenericTypeParameters(params Type[] genericTypeParameters)
    {
      if (genericTypeParameters != null)
      {
        this.GenericTypeParameters = genericTypeParameters.Select(b => b).ToArray();
      }
    }
    public virtual bool WithPermission { get; } = false;
    public virtual bool AddAssembly { get; } = false;
    public virtual bool IsServiceModule => true;
    public virtual decimal Index => 0;
    public abstract string ModuleTitle { get; }
    public abstract string ModuleName { get; }

    public virtual IEnumerable<Assembly> BlazorAssemblies()
    {
      return Enumerable.Empty<Assembly>();
    }
    public abstract Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default);


    public virtual IEnumerable<ModuleDependOn> Depends()
    {
      return Enumerable.Empty<ModuleDependOn>();
    }
    public virtual async Task<ContentResponse<RoleBasedPermissionDTO>> GetRoleBasedPermissionAsync(
      IContext? db, string? roleName, TokenDTO? user, CancellationToken ct = default)
    {
      var result = new ContentResponse<RoleBasedPermissionDTO>();
      if (db == null)
      {
        result.SetValidation(ValidationResultHelper.New("Context is Required", "context"));
        return result;
      }
      if (String.IsNullOrEmpty(roleName))
      {
        result.SetValidation(ValidationResultHelper.New("Role is Required", nameof(roleName)));
        return result;
      }
      var roleNormalization = roleName!.ToUpper();
      var moduleNormalization = ModuleName.ToUpper();
      var permissions = db.Query<RoleBasedPermission>(false)
        .Where(b => moduleNormalization.Equals(b.NormalizationModuleName) && roleNormalization.Equals(b.NormalizationRoleName))
        .ToArray();

      if (permissions.Length > 1)
      {
        var needDeletedPermissions = permissions.Where((b, i) => i > 0).ToArray();
        foreach (var d in needDeletedPermissions)
        {
          db.Delete<RoleBasedPermission>(d);
        }
        await db.SaveChangesAsync(db.User, ct);
      }
      RoleBasedPermission permission;
      if (permissions?.Length == 0)
      {
        permission = new RoleBasedPermission()
        {

        };
      }
      else
      {
        permission = permissions!.FirstOrDefault()!;
      }
      var permissionsList = await GetPermissions(db, user, ct);

      var dto = new RoleBasedPermissionDTO()
      {
        ModuleName = ModuleName,
        RoleName = roleName.ToUpper(),
        Items = permissionsList.Select(b => new StandardProperty[] {
          new StandardProperty()
          {
            PropertyName = b,
            Label = b,
            InputType = EnumInputType.Switch,
            Value=String.Empty,
            Col = 3,
            ColType = "md"
          }}).SelectMany(b => b).ToArray()
      };
      result.SetSuccess(dto);
      var trueString = true.StringValue();
      foreach (var v in dto.Items.Where(b => permission.PermissionList.Any(s => String.Equals(b.PropertyName, s, StringComparison.OrdinalIgnoreCase))))
      {
        v.Value = trueString;
      }

      return result;
    }

    public virtual async Task<ContentResponse<bool>> UpdateRoleBasedPermissionAsync(
      IContext? db, string? roleName, RoleBasedPermissionDTO? dto, TokenDTO? user, bool save = true, CancellationToken ct = default)
    {
      var result = new ContentResponse<bool>();
      if (db == null)
      {
        result.SetValidation(ValidationResultHelper.New("Context is Required", "context"));
        return result;
      }
      if (String.IsNullOrEmpty(roleName))
      {
        result.SetValidation(ValidationResultHelper.New("Role is Required", nameof(roleName)));
        return result;
      }
      if (dto == null)
      {
        result.SetValidation(ValidationResultHelper.New("dto is Required", nameof(dto)));
        return result;
      }
      if (dto?.Items == null)
      {
        result.SetValidation(ValidationResultHelper.New("Items is null", nameof(dto.Items)));
        return result;
      }
      var roleNameNormalization = roleName.ToUpper();
      var moduleNormalization = ModuleName.ToUpper();
      var permissions = db.Query<RoleBasedPermission>(false).Where(b =>
          moduleNormalization.Equals(b.NormalizationModuleName) && roleNameNormalization.Equals(b.NormalizationRoleName)).ToArray();
      if (permissions.Length > 1)
      {
        var needDeletedPermissions = permissions.Where((b, i) => i > 0).ToArray();
        foreach (var d in needDeletedPermissions)
        {
          db.Delete<RoleBasedPermission>(d);
        }
      }
      RoleBasedPermission? permission = null;
      var permissionsFromDto = dto!.Items!.Where(b =>
      {
        var val = b!.Value!.GetObjValue<bool>();
        return (val.Success && val.Content);
      }).Select(b => b.PropertyName?.Trim() ?? "").Where(b => !String.IsNullOrEmpty(b)).Distinct().ToArray();

      var modulePermissions = await GetPermissions(db, user, ct);

      var matchedPermissions = modulePermissions.Where(b =>
        permissionsFromDto.Any(c => String.Equals(c, b, StringComparison.OrdinalIgnoreCase)));

      var permissionString = matchedPermissions.BackToString();
      if (permissions.Length < 1)
      {
        var newPermission = new RoleBasedPermission()
        {
          ModuleName = ModuleName,
          RoleName = roleNameNormalization,
          Permissions = permissionString,
        };
        await db.AddAsync<RoleBasedPermission>(newPermission, ct);
      }
      else
      {
        permission = permissions[0];
        permission.Permissions = permissionString;

      }
      if (save)
      {
        await db.SaveChangesAsync(user, ct);
      }
      var cache = CacheBaseHelper.GetCache(this.GetType());
      var tenantId = db.TenantID?.ToString() ?? "";
      Func<string, string> cacheKey = b => $"{tenantId}_{b}";
      cache?.Set<IEnumerable<string>>(cacheKey(roleName.ToUpper()), matchedPermissions.ToArray(), -1);
      return result;

    }

    public virtual async Task<ContentResponse<string>> GetRoleBasedPermissionAsync(IContext db,
      IEnumerable<string> roleNames, string token, CancellationToken ct = default)
    {
      var result = new ContentResponse<string>();
      var upperRoleName = roleNames.Select(b => b.ToUpper());
      if (upperRoleName?.Any() != true)
      {
        result.SetSuccess("");
        return result;
      }
      var asyncQuery = db.ServiceProvider!.GetService<IAsyncQuery>()!;
      var cachedPermission = db.ServiceProvider?.GetService<TokenManagement>()?.CachedPermission == true;
      var tenantId = db.TenantID?.ToString() ?? "";
      Func<string, string> cacheKey = b => $"{tenantId}_{b}";
      var typedCache = db.ServiceProvider?.GetMemoryKeyValueCacheByType(this.GetType());
      var roleInCache = upperRoleName.Select(b => (b, typedCache?.Get<IEnumerable<string>>(cacheKey(b)))).Where(b => b.Item2 != null);
      string[]? roleNameQuery = null;
      if (cachedPermission)
      {
        roleNameQuery = upperRoleName.Where(b => roleInCache.Select(b => b.b).Contains(b) != true).ToArray();
        if (roleNameQuery?.Length == 0)
        {
          var cacheValue = roleInCache.Where(b => b.Item2 != null).SelectMany(b => b.Item2!).Distinct();
          result.SetSuccess(
            string.Join(",", cacheValue));
          return result;
        }
      }
      else
      {
        roleNameQuery = upperRoleName.ToArray();
      }
      var roleInDb =
              (await asyncQuery.ToArrayAsync(db.Query<RoleBasedPermission>(true)
              .Where(b => b.ModuleName == ModuleName && roleNameQuery.Any(c => c == b.NormalizationRoleName)), ct))

              .Select(b => (b.RoleName, b.PermissionList))
              .DistinctBy(b => b.RoleName)
              .ToArray();
      if (roleInDb.Length <= 0)
      {
        if (!cachedPermission)
        {
          result.SetSuccess("");
          return result;
        }
      }

      if (cachedPermission)
      {
        foreach (var role in roleNameQuery)
        {
          var roleQuery = roleInDb.Where(b => b.RoleName == role).Select(b => b.PermissionList).FirstOrDefault();
          if (roleQuery != null)
          {
            typedCache?.Set<IEnumerable<string>>(cacheKey(role), roleQuery.ToArray(), -1);
          }
          else
          {
            typedCache?.Set<IEnumerable<string>>(cacheKey(role), Enumerable.Empty<string>(), -1);
          }
        }
        var finallCache = upperRoleName.Select(b => (b, typedCache?.Get<IEnumerable<string>>(cacheKey(b)))).Where(b => b.Item2 != null).SelectMany(b => b.Item2!).Distinct();
        result.SetSuccess(
          string.Join(",", finallCache));
      }
      else
      {
        var r = roleInDb.SelectMany(b => b.PermissionList).Distinct();
        result.SetSuccess(
          string.Join(",", r));
      }


      return result;
    }

    public virtual IEnumerable<Type> SharedEntityTypes => Enumerable.Empty<Type>();
    public virtual IEnumerable<SharedModule> SharedModuleTypes => Enumerable.Empty<SharedModule>();

    public virtual bool SkipAutoServiceRegistration { get; private set; }

    public virtual void Constructor(ServiceConfigurationContext context)
    {

    }
    public virtual Task PreConfigureServicesAsync(ServiceConfigurationContext context)
    {
      return Task.CompletedTask;
    }
    public virtual Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      var type = this.GetType();
      CmfMemoryCasheHelper.AddMemoryKeyValueCacheByType(context.Services!, type, null);
      return Task.CompletedTask;
    }
    public virtual Task PostConfigureServicesAsync(ServiceConfigurationContext context)
    {
      return Task.CompletedTask;
    }
    public virtual Task BeforePreApplicationInitializationAsync(ServiceConfigurationContext context)
    {
      return Task.CompletedTask;
    }
    public virtual Task PreApplicationInitializationAsync(ServiceConfigurationContext context)
    {
      return Task.CompletedTask;
    }
    public virtual Task ApplicationInitializationAsync(ServiceConfigurationContext context)
    {
      return Task.CompletedTask;
    }
    public virtual Task PostApplicationInitializationAsync(ServiceConfigurationContext context)
    {
      return Task.CompletedTask;
    }

    public virtual void FiltersConfiguration(FilterCollection filters)
    {

    }
    public virtual void AuthenticationConfiguration(ServiceConfigurationContext context)
    {

    }
    public virtual void JsonConfiguration(ServiceConfigurationContext context)
    {

    }
    public virtual void EndpointRouteBuilder(IEndpointRouteBuilder endpoints, ServiceConfigurationContext? context = null)
    {

    }

    public virtual void SwaggerConfiguration(SwaggerGenOptions setupAction)
    {

    }
    public virtual void SwaggerConfigurationWithApiVersion(
      SwaggerGenOptions setupAction, IApiVersionDescriptionProvider provider, ISwaggerApiVersion swaggerApiVersion)
    {

    }
    public virtual Task<ContentResponse<IEnumerable<NavActionItemDTO>>> GetMenu()
    {
      return Task.FromResult(
       new ContentResponse<IEnumerable<NavActionItemDTO>>()
       {
         Success = true,
         Content = Enumerable.Empty<NavActionItemDTO>()
       });
    }

    public virtual bool RunningInFinal { get; }
    public virtual Task FinalRootConfigure(ServiceConfigurationContext context)
    {
      return Task.CompletedTask;
    }


    public virtual Action<object>? ServiceOnModelCreating { get; }
    public virtual Action<object>? ServiceOnConfiguring { get; }

    public virtual IEnumerable<(Type, string)> EntityForContext()
    {
      return Enumerable.Empty<(Type, string)>();
    }

    public static void AddServiceModuleEntity<T>() where T : class
    {
      AddServiceModuleEntity(typeof(T));
    }
    public static void AddServiceModuleEntity(Type type)
    {
      ReflectPool.EntityNameMapping.TryAdd(type.Name, type);
      ReflectPool.SetQueryAndFindCheck(type);
      var map = type.GetMap();
      var id = map.Properties.Where(b =>
          b.HasCustomAttribute<KeyAttribute>() ||
          b.HasCustomAttribute(nameof(PersonalDataAttribute)) ||
          b.Name == nameof(IId<long>.Id)).FirstOrDefault();
      if (id != null)
      {
        ReflectPool.EntityKeyMapping.AddOrUpdate(type, id, (c, b) => id);
      }

      var queryFirstProperty = map.Properties.Where(b => b.HasCustomAttribute<QueryBeforeFilterAttribute>())
        .Select(b => b.Name).ToArray();
      ReflectPool.EntityTypeQueryFirst.TryAdd(type, queryFirstProperty);
    }
  }

}
