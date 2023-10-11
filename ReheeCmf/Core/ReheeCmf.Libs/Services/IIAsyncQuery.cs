using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Services
{
  public interface IAsyncQuery
  {
    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken ct);
    IQueryable<T> AsNoTracking<T>(IQueryable<T> query) where T : class;
    Task<T[]> ToArrayAsync<T>(IQueryable<T> query, CancellationToken ct);
  }
}
