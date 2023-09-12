using Microsoft.AspNetCore.Mvc;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons.Consts;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Modules;
using ReheeCmf.Modules.Controllers;
using ReheeCmf.Modules.Options;
using ReheeCmf.StandardInputs.Properties;

namespace ReheeCmf.AuthenticationModule.Controllers.v1._0
{
  [ApiController]
  [Route("Api/ModuleService")]
  public class ModuleServiceController : ReheeCmfController
  {
    
    private ServiceModule[] modules;

    public ModuleServiceController(IServiceProvider sp) : base(sp)
    {
     modules = ModuleOption.GetModuleBases(sp);
    }

    [HttpGet("{roleName}")]
    [CmfAuthorize(EntityNameForce = ConstAuthentication.PermissionReadRoleBasedAccess, EntityRoleBase = true)]
    public async Task<IActionResult> Get(string roleName, CancellationToken ct)
    {
      var property = new Dictionary<string, StandardProperty[]>();
      foreach (var service in modules)
      {
        var propertyResponse = await service.GetRoleBasedPermissionAsync(
          context, roleName, currentUser, ct);
        if (!propertyResponse.Success || propertyResponse.Content == null)
        {
          continue;
        }
        property.TryAdd(service.ModuleTitle, propertyResponse!.Content!.Items!.ToArray());
      }

      return StatusCode(200, property);
    }

    [HttpPut("{roleName}")]
    [CmfAuthorize(EntityNameForce = ConstAuthentication.PermissionUpdateRoleBasedAccess, EntityRoleBase = true)]
    public async Task<IActionResult> Put(
      string roleName, Dictionary<string, StandardProperty[]> properties, CancellationToken ct)
    {
      foreach (var s in modules)
      {
        var title = s.ModuleTitle;
        if (!properties.TryGetValue(title, out var item))
        {
          continue;
        }
        await s.UpdateRoleBasedPermissionAsync(context,
          roleName, new RoleBasedPermissionDTO()
          {
            Items = item.ToArray()
          }, currentUser, false, ct);
        await context!.SaveChangesAsync(currentUser, ct);
      }
      return StatusCode(200, true);
    }
  }
}
