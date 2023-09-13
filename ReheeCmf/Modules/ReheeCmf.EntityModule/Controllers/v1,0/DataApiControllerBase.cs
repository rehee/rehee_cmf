using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons;
using ReheeCmf.Entities;
using ReheeCmf.Helpers;
using ReheeCmf.Modules.Controllers;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ReheeCmf.EntityModule.Controllers.v1_0
{
  public class DataApiControllerBase<T, TKey> : ReheeCmfController where T : class, IId<TKey> where TKey : IEquatable<TKey>
  {
    public DataApiControllerBase(IServiceProvider sp) : base(sp)
    {
    }


    [EnableQuery()]
    [HttpGet]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public IEnumerable<T> Query()
    {
      return context!.Query<T>(true);
    }
    [EnableQuery()]
    [HttpGet("{key}")]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public T FindEntity(TKey key, CancellationToken ct)
    {
      var result = context!.Query<T>(true).Where(b => b.Id.Equals(key)).FirstOrDefault();
      if (result == null)
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      return result!;
    }
    //[HttpGet("{entityName}/{key}/item")]
    //[CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    //public async Task<IActionResult> FindEntityItem(
    //  string entityName, string key, CancellationToken ct)
    //{
    //  var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
    //  if (entity == null)
    //  {
    //    return NotFound();
    //  }
    //  var idResponse = key.GetObjValue(entity.Value.keyType);
    //  if (!idResponse.Success || idResponse.Content == null)
    //  {
    //    return NotFound();
    //  }
    //  var result = context!.Find(entity.Value.entityType, idResponse.Content!);
    //  if (result == null)
    //  {
    //    return NotFound();
    //  }

    //  return Ok(StandardInputHelper.GetStandardProperty(result, context));
    //}

    //[HttpPost(CrudOption.DataEndpoint)]
    //[CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    //public async Task<IActionResult> CreateEntity(string entityName, Dictionary<string, string?> properties, CancellationToken ct)
    //{
    //  var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
    //  if (entity == null)
    //  {
    //    return NotFound();
    //  }

    //  var entityObject = Activator.CreateInstance(entity.Value.entityType);
    //  StandardInputHelper.UpdateProperty(entityObject, properties);
    //  context!.Add(entity.Value.entityType, entityObject);
    //  await context.SaveChangesAsync(currentUser, ct);
    //  if (entityObject is IStringId str)
    //  {
    //    return Ok(str.StringId());
    //  }
    //  return Ok();
    //}

    //[HttpPut(CrudOption.DataEndpoint)]
    //[CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    //public async Task<IActionResult> EditEntity(string entityName, string key, Dictionary<string, string?> properties, CancellationToken ct)
    //{
    //  var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
    //  if (entity == null)
    //  {
    //    return NotFound();
    //  }
    //  var idResponse = key.GetObjValue(entity.Value.keyType);
    //  if (!idResponse.Success || idResponse.Content == null)
    //  {
    //    return NotFound();
    //  }
    //  var result = context!.Find(entity.Value.entityType, idResponse.Content!);
    //  if (result == null)
    //  {
    //    return NotFound();
    //  }
    //  StandardInputHelper.UpdateProperty(result, properties);
    //  await context.SaveChangesAsync(currentUser, ct);
    //  return Ok();
    //}

    //[HttpPut("{entityName}")]
    //[CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    //public async Task<IActionResult> BulkEditEntity(string entityName, Dictionary<string, string?>[] properties, CancellationToken ct)
    //{
    //  var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
    //  if (entity == null)
    //  {
    //    return NotFound();
    //  }
    //  var idKey = nameof(IId<int>.Id);
    //  var validations = new List<ValidationResult>();
    //  foreach (var p in properties)
    //  {
    //    var key = p.Keys.FirstOrDefault(b => String.Equals(b, idKey, StringComparison.OrdinalIgnoreCase));
    //    if (!p.TryGetValue(key ?? idKey, out var idString) || String.IsNullOrEmpty(idString))
    //    {
    //      validations.Add(ValidationResultHelper.New("Id is required fields for bulkUpdate", idKey));
    //      continue;
    //    }
    //    var idResponse = idString.GetObjValue(entity.Value.keyType);
    //    if (!idResponse.Success || idResponse.Content == null)
    //    {
    //      validations.Add(ValidationResultHelper.New($"Entity for id {idString} is not available", idKey));
    //      continue;
    //    }
    //    var result = context!.Find(entity.Value.entityType, idResponse.Content!);
    //    if (result == null)
    //    {
    //      if (!idResponse.Success || idResponse.Content == null)
    //      {
    //        validations.Add(ValidationResultHelper.New($"Entity for id {idString} is not Exist", idKey));
    //        continue;
    //      }
    //    }
    //    if (validations.Count > 0)
    //    {
    //      continue;
    //    }
    //    StandardInputHelper.UpdateProperty(result, p);
    //  }
    //  if (validations.Count > 0)
    //  {
    //    StatusException.Throw(validations.ToArray());
    //  }
    //  await context!.SaveChangesAsync(currentUser, ct);
    //  return Ok();
    //}

    //[HttpDelete(CrudOption.DataEndpoint)]
    //[CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    //public async Task<IActionResult> DeleteEntityKey(string entityName, string key, CancellationToken ct)
    //{
    //  var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
    //  if (entity == null)
    //  {
    //    return NotFound();
    //  }
    //  var idResponse = key.GetObjValue(entity.Value.keyType);
    //  if (!idResponse.Success || idResponse.Content == null)
    //  {
    //    return NotFound();
    //  }
    //  var result = context!.Find(entity.Value.entityType, idResponse.Content!);
    //  if (result == null)
    //  {
    //    return NotFound();
    //  }
    //  context.Delete(entity.Value.entityType, idResponse.Content);
    //  await context!.SaveChangesAsync(currentUser, ct);
    //  return Ok();
    //}

    //[HttpDelete("{entityName}")]
    //[CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    //public async Task<IActionResult> DeleteEntityMultiKey(
    //  string entityName, string[] keys, CancellationToken ct)
    //{
    //  var entity = EntityRelationHelper.GetEntityTypeAndKey(entityName);
    //  if (entity == null)
    //  {
    //    return NotFound();
    //  }
    //  foreach (var key in keys)
    //  {
    //    var idResponse = key.GetObjValue(entity.Value.keyType);
    //    if (!idResponse.Success || idResponse.Content == null)
    //    {
    //      return NotFound();
    //    }
    //    var result = context!.Find(entity.Value.entityType, idResponse.Content!);
    //    if (result == null)
    //    {
    //      return NotFound();
    //    }
    //    context.Delete(entity.Value.entityType, idResponse.Content);
    //  }
    //  await context!.SaveChangesAsync(currentUser, ct);
    //  return Ok();
    //}

  }
}
