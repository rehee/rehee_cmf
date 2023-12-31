﻿using Microsoft.AspNetCore.Mvc;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons.Consts;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Modules;
using ReheeCmf.Modules.Controllers;
using ReheeCmf.Modules.Options;
using ReheeCmf.Modules.Permissions;
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
    [CmfAuthorize(EntityNameForce = ConstCmfAuthenticationModule.ReadRoleBasedAccess, EntityRoleBase = true)]
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
    [CmfAuthorize(EntityNameForce = ConstCmfAuthenticationModule.UpdateRoleBasedAccess, EntityRoleBase = true)]
    public async Task<IActionResult> Put(
      string roleName, Dictionary<string, StandardProperty[]> properties, CancellationToken ct)
    {
      foreach (var s in modules)
      {
        var title = s.ModuleTitle;
        var key = properties.Keys.FirstOrDefault(b => String.Equals(title, b, StringComparison.OrdinalIgnoreCase));
        if (!properties.TryGetValue(key ?? title, out var item))
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
