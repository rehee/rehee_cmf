using Microsoft.AspNetCore.Mvc;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons;
using ReheeCmf.Entities;
using ReheeCmf.Helpers;
using ReheeCmf.Modules.Controllers;
using System.ComponentModel.DataAnnotations;

namespace ReheeCmf.EntityModule.Controllers.v1_0
{
  [ApiController]
  [Route("Api/Data")]
  public class DataApiController : ReheeCmfController
  {
    public DataApiController(IServiceProvider sp) : base(sp)
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
      var idResponse = key.GetObjValue(entity.Value.keyType);
      if (!idResponse.Success || idResponse.Content == null)
      {
        return NotFound();
      }
      var result = context!.Find(entity.Value.entityType, idResponse.Content!);
      if (result == null)
      {
        return NotFound();
      }
      return Ok(result);
    }
    [HttpGet("{entityName}/{key}/item")]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public async Task<IActionResult> FindEntityItem(
      string entityName, string key, CancellationToken ct)
    {
      var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
      if (entity == null)
      {
        return NotFound();
      }
      var idResponse = key.GetObjValue(entity.Value.keyType);
      if (!idResponse.Success || idResponse.Content == null)
      {
        return NotFound();
      }
      var result = context!.Find(entity.Value.entityType, idResponse.Content!);
      if (result == null)
      {
        return NotFound();
      }

      return Ok(StandardInputHelper.GetStandardProperty(result, context));
    }

    [HttpPost(CrudOption.DataEndpoint)]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public async Task<IActionResult> CreateEntity(string entityName, Dictionary<string, string?> properties, CancellationToken ct)
    {
      var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
      if (entity == null)
      {
        return NotFound();
      }

      var entityObject = Activator.CreateInstance(entity.Value.entityType);
      StandardInputHelper.UpdateProperty(entityObject, properties);
      context!.Add(entity.Value.entityType, entityObject);
      await context.SaveChangesAsync(currentUser, ct);
      if (entityObject is IStringId str)
      {
        return Ok(str.StringId());
      }
      return Ok();
    }

    [HttpPut(CrudOption.DataEndpoint)]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public async Task<IActionResult> EditEntity(string entityName, string key, Dictionary<string, string?> properties, CancellationToken ct)
    {
      var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
      if (entity == null)
      {
        return NotFound();
      }
      var idResponse = key.GetObjValue(entity.Value.keyType);
      if (!idResponse.Success || idResponse.Content == null)
      {
        return NotFound();
      }
      var result = context!.Find(entity.Value.entityType, idResponse.Content!);
      if (result == null)
      {
        return NotFound();
      }
      StandardInputHelper.UpdateProperty(result, properties);
      await context.SaveChangesAsync(currentUser, ct);
      return Ok();
    }

    [HttpPut("{entityName}")]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public async Task<IActionResult> BulkEditEntity(string entityName, Dictionary<string, string?>[] properties, CancellationToken ct)
    {
      var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
      if (entity == null)
      {
        return NotFound();
      }
      var idKey = nameof(IId<int>.Id);
      var validations = new List<ValidationResult>();
      foreach (var p in properties)
      {
        var key = p.Keys.FirstOrDefault(b => String.Equals(b, idKey, StringComparison.OrdinalIgnoreCase));
        if (!p.TryGetValue(key ?? idKey, out var idString) || String.IsNullOrEmpty(idString))
        {
          validations.Add(ValidationResultHelper.New("Id is required fields for bulkUpdate", idKey));
          continue;
        }
        var idResponse = idString.GetObjValue(entity.Value.keyType);
        if (!idResponse.Success || idResponse.Content == null)
        {
          validations.Add(ValidationResultHelper.New($"Entity for id {idString} is not available", idKey));
          continue;
        }
        var result = context!.Find(entity.Value.entityType, idResponse.Content!);
        if (result == null)
        {
          if (!idResponse.Success || idResponse.Content == null)
          {
            validations.Add(ValidationResultHelper.New($"Entity for id {idString} is not Exist", idKey));
            continue;
          }
        }
        if (validations.Count > 0)
        {
          continue;
        }
        StandardInputHelper.UpdateProperty(result, p);
      }
      if (validations.Count > 0)
      {
        StatusException.Throw(validations.ToArray());
      }
      await context!.SaveChangesAsync(currentUser, ct);
      return Ok();
    }

    [HttpDelete(CrudOption.DataEndpoint)]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public async Task<IActionResult> DeleteEntityKey(string entityName, string key, CancellationToken ct)
    {
      var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
      if (entity == null)
      {
        return NotFound();
      }
      var idResponse = key.GetObjValue(entity.Value.keyType);
      if (!idResponse.Success || idResponse.Content == null)
      {
        return NotFound();
      }
      var result = context!.Find(entity.Value.entityType, idResponse.Content!);
      if (result == null)
      {
        return NotFound();
      }
      context.Delete(entity.Value.entityType, idResponse.Content);
      await context!.SaveChangesAsync(currentUser, ct);
      return Ok();
    }

    [HttpDelete("{entityName}")]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public async Task<IActionResult> DeleteEntityMultiKey(
      string entityName, string[] keys, CancellationToken ct)
    {
      var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
      if (entity == null)
      {
        return NotFound();
      }
      foreach (var key in keys)
      {
        var idResponse = key.GetObjValue(entity.Value.keyType);
        if (!idResponse.Success || idResponse.Content == null)
        {
          return NotFound();
        }
        var result = context!.Find(entity.Value.entityType, idResponse.Content!);
        if (result == null)
        {
          return NotFound();
        }
        context.Delete(entity.Value.entityType, idResponse.Content);
      }
      await context!.SaveChangesAsync(currentUser, ct);
      return Ok();
    }
  }
}
