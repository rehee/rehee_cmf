using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Commons;
using ReheeCmf.Contexts;
using ReheeCmf.DTOProcessors.Processors;
using ReheeCmf.DTOProcessors;
using ReheeCmf.Entities;
using ReheeCmf.Responses;
using ReheeCmf.Tenants;
using ReheeCmf.Caches;
using ReheeCmf.Services;
using ReheeCmf.Helpers;
using Microsoft.AspNetCore.OData.Results;

namespace ReheeCmf.EntityModule.Controllers.v1_0
{
  public abstract class CmfDTOOOataController<T> : ODataController, IDisposable, IAsyncDisposable, IWithEntityName where T : IQueryKey
  {
    public CmfDTOOOataController(IServiceProvider sp)
    {
      this.sp = sp;
      this.db = sp.GetService<IContext>();
      this.auth = sp.GetService<IAuthorize>();
      this.crudOption = sp.GetService<CrudOption>();
      this.queryMemoryCache = sp.GetService<IContextScope<QuerySecondCache>>();
      this.tenantDetail = sp.GetService<IContextScope<Tenant>>();
      this.scopeUser = sp.GetService<IContextScope<TokenDTO>>();
      tokenDTO = scopeUser.Value;
      scopeUser.ValueChange += ScopeUser_ValueChange;
      queryService = sp.GetService<ITypeQuery<T>>();
      EntityName = typeof(T).Name;
      this.asyncQuery = sp.GetService<IAsyncQuery>()!;
    }
    protected readonly IAsyncQuery asyncQuery;
    protected readonly ITypeQuery<T> queryService;
    public string EntityName { get; protected set; }
    [EnableQuery]
    [HttpGet]
    [CmfAuthorize(EntityName = "dtoName", PermissionClass = true)]
    public virtual IEnumerable<T> Get()
    {
      return queryService.QueryWithType(CurrentUser);
    }
    [HttpGet("{queryKey}")]
    [EnableQuery()]
    [CmfAuthorize(EntityName = "dtoName", PermissionClass = true)]
    public virtual SingleResult<T> Find(string queryKey, CancellationToken ct)
    {
      return SingleResult.Create<T>(
        queryService.QueryWithType(CurrentUser).Where(b => b.QueryKey == queryKey)
        );
    }


    [HttpPost()]
    [CmfAuthorize(EntityName = "dtoName", PermissionClass = true)]
    public virtual async Task<IActionResult> Post([FromBody] object model, CancellationToken ct)
    {
      if (queryService is IDtoProcessor<T> processor)
      {
        processor.Initialization(model.ToString(), CurrentUser);
        var validate = processor.ValidationrResponse();
        if (validate.ValidationError)
        {
          StatusException.Throw(validate.Validation.ToArray());
        }
        await processor.CreateAsync(ct);
        await processor.SaveChangesAsync(CurrentUser, ct);
        await processor.AfterCreateAsync(ct);
        return Ok(processor.CreateResponse);
      }
      return BadRequest();
    }

    [HttpPut("{key}")]
    [CmfAuthorize(EntityName = "dtoName", PermissionClass = true)]
    public virtual async Task<IActionResult> Put(string key, [FromBody] object model, CancellationToken ct)
    {
      if (queryService is IDtoProcessor<T> processor)
      {
        await processor.InitializationAsync(key, model.ToString(), CurrentUser, ct);
        var validate = processor.ValidationrResponse();
        if (validate.ValidationError)
        {
          StatusException.Throw(validate.Validation.ToArray());
        }
        await processor.UpdateAsync(ct);
        await processor.SaveChangesAsync(CurrentUser, ct);
        return Ok();
      }
      return Ok();
    }

    [HttpDelete("{key}")]
    [CmfAuthorize(EntityName = "dtoName", PermissionClass = true)]
    public virtual async Task<IActionResult> Delete(string key, CancellationToken ct)
    {

      if (queryService is IDtoProcessor<T> processor)
      {
        processor.Initialization(CurrentUser);
        await processor.DeleteAsync(key, ct);
        await processor.SaveChangesAsync(CurrentUser, ct);
        return Ok();
      }
      return BadRequest();
    }

    #region commoncode
    private void ScopeUser_ValueChange(object sender, ContextScopeEventArgs<TokenDTO> e)
    {
      tokenDTO = e.Value;
    }

    public virtual bool FlagOnly => false;
    protected readonly IServiceProvider sp;
    protected readonly IContext db;
    protected readonly IAuthorize auth;
    protected readonly CrudOption crudOption;
    protected readonly IContextScope<QuerySecondCache> queryMemoryCache;
    protected readonly IContextScope<Tenant> tenantDetail;
    protected readonly IContextScope<TokenDTO> scopeUser;
    protected TokenDTO? tokenDTO { get; set; }
    public Tenant? Tenant => tenantDetail?.Value;
    public Guid? TenantID => Tenant?.TenantID;


    protected TokenDTO CurrentUser => tokenDTO;
    public bool TenantSet { get; set; }

    protected Task<TokenDTO> GetUserInfo()
    {
      return Task.FromResult(tokenDTO);
    }
    protected TokenDTO GetUserInfoSync()
    {
      return tokenDTO;
    }
    protected string GetToken()
    {
      return tokenDTO?.TokenString ?? String.Empty;
    }

    protected IActionResult ReturnResponse<T>(IContentResponse<T> response)
    {
      if (response == null)
      {
        return BadRequest();
      }
      if (response.Success)
      {
        return StatusCode((int)response.Status, response.Content);
      }
      return StatusCode((int)response.Status, response.ErrorMessage);
    }
    protected IActionResult ReturnResponse(IContentResponse<object> response)
    {
      return ReturnResponse<object>(response);
    }



    [NonAction]
    public virtual ValueTask DisposeAsync()
    {
      this.Dispose();
      return ValueTask.CompletedTask;
    }
    protected virtual bool IsDispose { get; set; }
    [NonAction]
    public void Dispose()
    {
      if (IsDispose)
      {
        return;
      }
      IsDispose = true;
      EntityName = null;
      scopeUser.ValueChange -= ScopeUser_ValueChange;
      tokenDTO = null;
    }
    #endregion
  }
}
