using ReheeCmf.Commons.Jsons.Options;
using ReheeCmf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReheeCmf.DTOProcessors.Processors
{
  public abstract class DtoProcessor<T> : IDtoProcessor<T> where T : IQueryKey
  {
    protected readonly IContext context;
    protected readonly IAsyncQuery asyncQuery;

    public string TypeName => Type.Name;
    public Type Type => typeof(T);
    public DtoProcessor(IContext context, IAsyncQuery asyncQuery)
    {
      this.context = context;
      this.asyncQuery = asyncQuery;
    }
    public string? CreateResponse { get; protected set; }

    public T? dto { get; set; }
    protected string? rawJson { get; set; }
    protected T? oldDto { get; set; }
    protected TokenDTO? currentUser { get; set; }
    public void Initialization(TokenDTO? user)
    {
      currentUser = user;
    }
    public void Initialization(string? json, TokenDTO? user)
    {
      Initialization(user);
      rawJson = json;
      dto = JsonSerializer.Deserialize<T>(json ?? "", JsonOption.DefaultOption);
    }
    public virtual async Task InitializationAsync(string? key, string? json, TokenDTO? user, CancellationToken ct)
    {
      oldDto = await FindAsyncWithType(user, key, ct);
      if (oldDto == null)
      {
        throw new StatusException("", HttpStatusCode.NotFound);
      }
      Initialization(json, user);
      dto = JsonHelper.MergePatch(oldDto, rawJson!);
    }

    public virtual async Task<object> FindAsync(TokenDTO user, string? queryKey, CancellationToken ct)
    {
      return await FindAsyncWithType(user, queryKey, ct);
    }
    public virtual async Task<T> FindAsyncWithType(TokenDTO? user, string? queryKey, CancellationToken ct)
    {
      var query = QueryWithType(user);
      return await asyncQuery.FirstOrDefaultAsync(QueryWithType(user).Where(b => b.QueryKey == queryKey), ct);
    }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var validation = new List<ValidationResult>();
      Validator.TryValidateObject(dto, new ValidationContext(dto), validation, true);
      return validation;
    }
    public virtual int SaveChanges(TokenDTO user)
    {
      if (context == null)
      {
        throw new NullReferenceException();
      }
      return context.SaveChanges(user);
    }
    public virtual async Task<int> SaveChangesAsync(TokenDTO user, CancellationToken cancellationToken = default)
    {
      if (context == null)
      {
        throw new NullReferenceException();
      }
      return await context.SaveChangesAsync(user, cancellationToken);
    }


    #region non implement method
    public virtual IQueryable Query(TokenDTO user)
    {
      return QueryWithType(user);
    }
    public virtual IQueryable Query(TokenDTO user, string key)
    {
      return QueryWithType(user).Where(b => b.QueryKey == key);
    }
    public virtual IQueryable<T> QueryWithType(TokenDTO user)
    {
      throw new NotImplementedException();
    }
    public virtual Task<bool> CreateAsync(CancellationToken ct)
    {
      throw new NotImplementedException();
    }
    public virtual Task<bool> AfterCreateAsync(CancellationToken ct)
    {
      throw new NotImplementedException();
    }
    public virtual Task<bool> UpdateAsync(CancellationToken ct)
    {
      throw new NotImplementedException();
    }

    public virtual Task<bool> DeleteAsync(string key, CancellationToken ct)
    {
      throw new NotImplementedException();
    }

    public virtual void Dispose()
    {
      dto = default(T?);
      rawJson = null;
      oldDto = default(T?);
      currentUser = null;
    }

    public ValueTask DisposeAsync()
    {
      Dispose();
      return ValueTask.CompletedTask;
    }

    #endregion
  }
}
