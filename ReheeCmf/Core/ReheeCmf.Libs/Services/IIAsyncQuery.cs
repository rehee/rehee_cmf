using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Services
{
  public interface IIAsyncQuery
  {
    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken ct);
    
  }
}
