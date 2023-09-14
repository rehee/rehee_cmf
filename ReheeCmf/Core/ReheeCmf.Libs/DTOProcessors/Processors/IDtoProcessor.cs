using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.DTOProcessors.Processors
{
  public interface IDtoProcessor : ISaveChange, IValidatableObject, IFindByQueryKey, ITypeQuery, IDisposable, IAsyncDisposable, IDtoCreateProcessor, IDtoUpdateProcessor, IDtoDeleteProcessor
  {
    void Initialization(TokenDTO? user);
    void Initialization(string? json, TokenDTO? user);
    Task InitializationAsync(string? key, string? json, TokenDTO? user, CancellationToken ct);

  }
  public interface IDtoProcessor<T> : IDtoProcessor, ITypeQuery<T>, IFindByQueryKey<T>, ITypedCreateProcessor<T>, IDtoUpdateProcessor<T>, ITypedDeleteProcessor<T> where T : IQueryKey
  {

  }
}
