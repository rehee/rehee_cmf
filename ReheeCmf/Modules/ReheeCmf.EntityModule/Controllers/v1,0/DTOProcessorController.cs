using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons;
using ReheeCmf.DTOProcessors.Processors;
using ReheeCmf.Modules.Controllers;
using ReheeCmf.ODatas.Commons;
using System.Net;
using ReheeCmf.Helpers;
using ReheeCmf.Caches;

namespace ReheeCmf.EntityModule.Controllers.v1_0
{
  [ApiController]
  [Route(CrudOption.DTOProcessor)]
  public class DTOProcessorController : ReheeCmfController
  {
    public DTOProcessorController(IServiceProvider sp) : base(sp)
    {
    }

    private string getkeyFromDto(string dtoName)
    {
      return ODataPools.QueryNameTypeMapping.Keys.FirstOrDefault(b => b.Equals(dtoName, StringComparison.OrdinalIgnoreCase));
    }
    [HttpGet("{dtoName}/json")]
    [EnableQuery()]
    [CmfAuthorize(EntityName = "dtoName", PermissionClass = true)]
    public virtual object Query(string dtoName)
    {
      Type type = null;
      queryMemoryCache.SetValue(new QuerySecondCache(true));
      if (String.IsNullOrEmpty(dtoName) ||
        !ODataPools.QueryNameTypeMapping.TryGetValue(getkeyFromDto(dtoName), out type))
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      var service = serviceProvider.GetService(type);
      if (serviceProvider.GetService(type) is ITypeQuery qb)
      {
        return qb.Query(currentUser);
      }
      return Ok(dtoName);
    }
    [HttpGet("{dtoName}/{queryKey}/json")]
    [EnableQuery()]
    [CmfAuthorize(EntityName = "dtoName", PermissionClass = true)]
    public virtual async Task<IActionResult> Find(string dtoName, string queryKey, CancellationToken ct)
    {
      Type type = null;
      queryMemoryCache.SetValue(new QuerySecondCache(true));
      if (String.IsNullOrEmpty(dtoName) ||
        !ODataPools.QueryNameTypeMapping.TryGetValue(getkeyFromDto(dtoName), out type))
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      var service = serviceProvider.GetService(type);
      if (service is IFindByQueryKey find)
      {
        return Ok(await find.FindAsync(currentUser, queryKey, ct));
      }
      if (service is ITypeQuery qb)
      {
        return Ok(qb.Query(currentUser));
      }
      return Ok(dtoName);
    }


    [HttpPost("{dtoName}")]
    [CmfAuthorize(EntityName = "dtoName", PermissionClass = true)]
    public virtual async Task<IActionResult> Post(string dtoName, [FromBody] object model, CancellationToken ct)
    {
      Type type = null;
      if (String.IsNullOrEmpty(dtoName) ||
        !ODataPools.QueryNameTypeMapping.TryGetValue(getkeyFromDto(dtoName), out type))
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      var service = serviceProvider.GetService(type);
      if (service is IDtoProcessor processor)
      {
        processor.Initialization(model.ToString(), currentUser);
        var validate = processor.ValidationrResponse();
        if (validate.ValidationError)
        {
          StatusException.Throw(validate.Validation.ToArray());
        }
        await processor.CreateAsync(ct);
        await processor.SaveChangesAsync(currentUser, ct);
        await processor.AfterCreateAsync(ct);
        return Ok(processor.CreateResponse);
      }
      return BadRequest();
    }

    [HttpPut("{dtoName}/{key}")]
    [CmfAuthorize(EntityName = "dtoName", PermissionClass = true)]
    public virtual async Task<IActionResult> Put(string dtoName, string key, [FromBody] object model, CancellationToken ct)
    {
      Type type = null;
      if (String.IsNullOrEmpty(dtoName) || String.IsNullOrEmpty(dtoName) ||
        !ODataPools.QueryNameTypeMapping.TryGetValue(getkeyFromDto(dtoName), out type))
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      var service = serviceProvider.GetService(type);
      if (service is IDtoProcessor processor)
      {
        await processor.InitializationAsync(key, model.ToString(), currentUser, ct);
        var validate = processor.ValidationrResponse();
        if (validate.ValidationError)
        {
          StatusException.Throw(validate.Validation.ToArray());
        }
        await processor.UpdateAsync(ct);
        await processor.SaveChangesAsync(currentUser, ct);
        return Ok();
      }
      return Ok();
    }

    [HttpDelete("{dtoName}/{key}")]
    [CmfAuthorize(EntityName = "dtoName", PermissionClass = true)]
    public virtual async Task<IActionResult> Delete(string dtoName, string key, CancellationToken ct)
    {
      Type type = null;
      if (String.IsNullOrEmpty(dtoName) || String.IsNullOrEmpty(key) ||
        !ODataPools.QueryNameTypeMapping.TryGetValue(getkeyFromDto(dtoName), out type))
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      var service = serviceProvider.GetService(type);
      if (service is IDtoProcessor processor)
      {
        processor.Initialization(currentUser);
        await processor.DeleteAsync(key, ct);
        await processor.SaveChangesAsync(currentUser, ct);
        return Ok();
      }
      return BadRequest();
    }

  }
}
