using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.DTOProcessors.Processors
{
  public interface IDtoCreateProcessor
  {
    string? CreateResponse { get; }
    Task<bool> CreateAsync(CancellationToken ct);
    Task<bool> AfterCreateAsync(CancellationToken ct);
  }
  public interface ITypedCreateProcessor<T> : IDtoCreateProcessor
  {
  }
  public interface IDtoDeleteProcessor
  {
    Task<bool> DeleteAsync(string key, CancellationToken ct);
  }
  public interface ITypedDeleteProcessor<T> : IDtoDeleteProcessor
  {
  }
  public interface IDtoUpdateProcessor
  {
    Task<bool> UpdateAsync(CancellationToken ct);
  }
  public interface IDtoUpdateProcessor<T> : IDtoUpdateProcessor
  {

  }
}
