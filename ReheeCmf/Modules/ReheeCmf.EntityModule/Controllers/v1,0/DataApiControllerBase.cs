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
    public virtual IEnumerable<T> Query()
    {
      return context!.Query<T>(true);
    }
    [EnableQuery()]
    [HttpGet("{key}")]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public virtual T FindEntity(TKey key, CancellationToken ct)
    {
      var result = context!.Query<T>(true).Where(b => b.Id.Equals(key)).FirstOrDefault();
      if (result == null)
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      return result!;
    }
    [HttpGet("{key}/item")]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public virtual async Task<IActionResult> FindEntityItem(TKey key, CancellationToken ct)
    {
      var result = context!.Query<T>(true).Where(b => b.Id.Equals(key)).FirstOrDefault();
      if (result == null)
      {
        return Ok(StandardInputHelper.GetStandardProperty(Activator.CreateInstance(typeof(T))!, context));
      }
      return Ok(StandardInputHelper.GetStandardProperty(result!, context));
    }

    [HttpPost(CrudOption.DataEndpoint)]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public virtual async Task<IActionResult> CreateEntity(Dictionary<string, string?> properties, CancellationToken ct)
    {
      var entityObject = Activator.CreateInstance(typeof(T)) as T;
      StandardInputHelper.UpdateProperty(entityObject, properties);
      await context!.AddAsync<T>(entityObject!, ct);
      await context.SaveChangesAsync(currentUser, ct);
      return Ok(entityObject!.Id);
    }

    [HttpPut(CrudOption.DataEndpoint)]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public virtual async Task<IActionResult> EditEntity(TKey key, Dictionary<string, string?> properties, CancellationToken ct)
    {
      var result = context!.Query<T>(true).Where(b => b.Id.Equals(key)).FirstOrDefault();
      if (result == null)
      {
        return NotFound();
      }
      StandardInputHelper.UpdateProperty(result, properties);
      await context.SaveChangesAsync(currentUser, ct);
      return Ok();
    }

    [HttpPut()]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public virtual async Task<IActionResult> BulkEditEntity(Dictionary<string, string?>[] properties, CancellationToken ct)
    {
      var entityName = typeof(T).Name;
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
    public virtual async Task<IActionResult> DeleteEntityKey(TKey key, CancellationToken ct)
    {
      var result = context!.Query<T>(true).Where(b => b.Id.Equals(key)).FirstOrDefault();
      if (result == null)
      {
        return NotFound();
      }
      context.Delete<T>(result);
      await context!.SaveChangesAsync(currentUser, ct);
      return Ok();
    }

    [HttpDelete("{entityName}")]
    [CmfAuthorize(EntityName = "entityName", EntityRoleBase = true)]
    public virtual async Task<IActionResult> DeleteEntityMultiKey(TKey[] keys, CancellationToken ct)
    {
      var entities = context!.Query<T>(false).ToArray();
      if (entities.Length != keys.Length)
      {
        return NotFound();
      }
      foreach (var entity in entities)
      {
        context.Delete<T>(entity);
      }
      await context!.SaveChangesAsync(currentUser, ct);
      return Ok();
    }

  }
}
