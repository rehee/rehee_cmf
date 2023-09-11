
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ReheeCmf.Modules.ApiVersions;
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
    public virtual async Task<ContentResponse<RoleBasedPermissionDTO>> GetRoleBasedPermissionAsync(IContext? db,
      string? roleName, TokenDTO? user, CancellationToken ct = default)
    {
      if (db == null)
      {
        throw new ArgumentNullException();
      }
      var result = new ContentResponse<RoleBasedPermissionDTO>();
      var nomolizationRole = roleName.ToUpper();
      var permissions = db.Query<RoleBasedPermission>(false)
        .Where(b => b.ModuleName.Equals(ModuleName) && b.NormalizationRoleName.Equals(nomolizationRole))
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
      if (permissions?.Any() != true)
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
      result.SetSuccess(dto);
      var trueString = true.StringValue();
      foreach (var v in dto.Items.Where(b => permission.PermissionList.Any(s => String.Equals(b.PropertyName, s, StringComparison.OrdinalIgnoreCase))))
      {
        v.Value = trueString;
      }

      return result;
    }

    public virtual async Task<ContentResponse<bool>> UpdateRoleBasedPermissionAsync(IContext? db,
      string? roleName, RoleBasedPermissionDTO? dto, TokenDTO? user, bool save = true, CancellationToken ct = default)
    {
      var result = new ContentResponse<bool>();
      var roleNameNormalization = roleName.ToUpper();
      var permissions = db.Query<RoleBasedPermission>(false).Where(b =>
          b.ModuleName.Equals(ModuleName) && b.NormalizationRoleName.Equals(roleNameNormalization)).ToArray();
      if (permissions.Length > 1)
      {
        var needDeletedPermissions = permissions.Where((b, i) => i > 0).ToArray();
        foreach (var d in needDeletedPermissions)
        {
          db.Delete<RoleBasedPermission>(d);
        }
      }
      RoleBasedPermission permission = null;
      var permissionsFromDto = dto.Items.Where(b =>
      {
        var val = b.Value.GetValue<bool>();
        return (val.Success && val.Content);
      }).Select(b => b.PropertyName?.Trim() ?? "").Where(b => !String.IsNullOrEmpty(b)).Distinct().ToArray();

      var modulePermissions = await GetPermissions(db, user, ct);

      var matchedPermissions = modulePermissions.Where(b => permissionsFromDto.Any(c => String.Equals(c, b, StringComparison.OrdinalIgnoreCase)));

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

      return result;

    }

    public virtual Task<ContentResponse<string>> GetRoleBasedPermissionAsync(IContext db,
      IEnumerable<string> roleNames, string token, CancellationToken ct = default)
    {
      var result = new ContentResponse<string>();
      var upperRoleName = roleNames.Select(b => b.ToUpper());
      var roleInDb = db.Query<RoleBasedPermission>(true)
        .Where(b => b.ModuleName == ModuleName && upperRoleName.Any(c => c == b.NormalizationRoleName))
        .ToArray()
        .SelectMany(b => b.PermissionList)
        .Distinct()
        .ToArray();
      if (roleInDb.Length <= 0)
      {
        result.SetSuccess("");
        return Task.FromResult(result);
      }
      result.SetSuccess(
        string.Join(",", roleInDb));

      return Task.FromResult(result);
    }

    public virtual IEnumerable<Type> SharedEntityTypes => Enumerable.Empty<Type>();
    public virtual IEnumerable<SharedModule> SharedModuleTypes => Enumerable.Empty<SharedModule>();

    public virtual bool SkipAutoServiceRegistration { get; private set; }
    public ServiceConfigurationContext ServiceConfigurationContext { get; private set; }
    public void Constructor(ServiceConfigurationContext context)
    {
      ServiceConfigurationContext = context;

    }
    public virtual Task PreConfigureServicesAsync(ServiceConfigurationContext context)
    {
      return Task.CompletedTask;
    }
    public virtual Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      return Task.CompletedTask;
    }
    public virtual Task PostConfigureServicesAsync(ServiceConfigurationContext context)
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

    public virtual void FiltersConfigration(FilterCollection filters)
    {

    }
    public virtual void AuthenticationConfigration(ServiceConfigurationContext context)
    {

    }
    public virtual void JsonConfigration(ServiceConfigurationContext context)
    {

    }
    public virtual void EndpointRouteBuilder(IEndpointRouteBuilder endpoints, ServiceConfigurationContext context = null)
    {

    }

    public virtual void SwaggerConfigration(SwaggerGenOptions setupAction)
    {

    }
    public virtual void SwaggerConfigrationWithApiVersion(
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

    public virtual bool RuningInFinal { get; }
    public virtual async Task FinalRootConfigure(ServiceConfigurationContext context)
    {

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
      var id = type.GetMap().Properties.Where(b =>
          b.HasCustomAttribute<KeyAttribute>() ||
          b.HasCustomAttribute(nameof(PersonalDataAttribute)) ||
          b.Name == nameof(IId<long>.Id)).FirstOrDefault();
      ReflectPool.EntityKeyMapping.AddOrUpdate(type, id, (c, b) => id);
      var queryFirstProperty = type.GetMap().Properties.Where(b => b.HasCustomAttribute<QueryBeforeFilterAttribute>()).Select(b => b.Name).ToArray();
      ReflectPool.EntityTypeQueryFirst.TryAdd(type, queryFirstProperty);
    }
  }

}
