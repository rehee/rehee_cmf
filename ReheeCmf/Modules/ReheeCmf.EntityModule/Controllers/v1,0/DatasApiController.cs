using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons;
using ReheeCmf.Helpers;
using ReheeCmf.Modules.Controllers;
using ReheeCmf.Reflects.ReflectPools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.EntityModule.Controllers.v1_0
{
  [ApiController]
  [Route("Api/Data")]
  public class DatasApiController : ReheeCmfController
  {
    public DatasApiController(IServiceProvider sp) : base(sp)
    {
    }

    //[EnableQuery()]
    [HttpGet("{entityName}/Json")]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public IActionResult Query(string entityName)
    {
      //queryMemoryCache.SetValue(new QueryMemoryCache(true));
      var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
      if (entity == null)
      {
        return NotFound();
      }
      return Ok(context!.Query(entity.Value.entityType, true));
    }
    //[EnableQuery()]
    [HttpGet("{entityName}/{key}/Json")]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public IActionResult FindEntity(
      string entityName, string key, CancellationToken ct)
    {
      var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
      if (entity == null)
      {
        return NotFound();
      }
      var idResponse = key.GetValue(entity.Value.keyType);
      if (!idResponse.Success || idResponse.Content == null)
      {
        return NotFound();
      }
      var result = context!.Find(entity.Value.entityType, idResponse.Content!);
      return Ok(result);
    }
    //[HttpGet("{entityName}/{key}/item")]
    //[CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    //public async Task<IActionResult> FindEntityItem(
    //  string entityName, string key, CancellationToken ct)
    //{
    //  queryMemoryCache.SetValue(new QueryMemoryCache(true));
    //  return await FindEntityItemMethod(entityName, key, ct);
    //}

    //[HttpPut(CrudOption.DataEndpoint)]
    //[CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    //public async Task<IActionResult> EditEntity(
    //  string entityName, string key, Dictionary<string, string> properties, CancellationToken ct)
    //{
    //  return await EditMethod(entityName, key, properties, ct);
    //}
    //[HttpPut("{entityName}")]
    //[CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    //[HttpPost(CrudOption.DataEndpoint)]
    //[CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    //public async Task<IActionResult> CreateEntity(
    //      string entityName, Dictionary<string, string> properties, CancellationToken ct)
    //{
    //  return await CreateMthod(entityName, properties, ct);
    //}
    
    //[HttpDelete(CrudOption.DataEndpoint)]
    //[CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    //public async Task<IActionResult> DeleteEntityKey(
    //  string entityName, string key, CancellationToken ct)
    //{
    //  return await DeleteEntityMultiKeyMethod(entityName, new string[] { key }, ct);
    //}

    //[HttpDelete("{entityName}")]
    //[CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    //public async Task<IActionResult> DeleteEntityMultiKey(
    //  string entityName, string[] key, CancellationToken ct)
    //{
    //  var keys = key.Select(b => b.Trim()).ToArray();
    //  return await DeleteEntityMultiKeyMethod(entityName, keys, ct);
    //}
  }
}
